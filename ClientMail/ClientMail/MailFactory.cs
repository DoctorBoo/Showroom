using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.ComponentModel;
using System.Net;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.Office.Interop.Outlook;

namespace ClientMail
{
    public class OfficeFactory
    {
        object locker = new object();
        public OfficeFactory()
        {

        }
        public static bool mailSent = false;
        public static ConcurrentDictionary<string, string> _tokens;
        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            lock (locker)
            {
                // Get the unique identifier for this asynchronous operation.
                String token = (string)e.UserState;

                if (e.Cancelled)
                {
                    Console.WriteLine("[{0}] Send canceled.", token);
                }
                if (e.Error != null)
                {
                    Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
                }
                else
                {
                    Console.WriteLine("Message sent.");
                }
                mailSent = true;
                _tokens.TryAdd(token, token);
                Monitor.PulseAll(locker);
            }
            
        }
        public void Send(string server, int port, string userName, string password, object token)
        {
            lock (locker)
            {
                if (_tokens == null) _tokens = new ConcurrentDictionary<string, string>();

                // Command line argument must the the SMTP host.            
                SmtpClient client = new SmtpClient(server, port);
                // Credentials are necessary if the server requires the client  
                // to authenticate before it will send e-mail on the client's behalf.            
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(userName, password);
                // Specify the e-mail sender. 
                // Create a mailing address that includes a UTF8 character 
                // in the display name.
                MailAddress from = new MailAddress("jane@contoso.com",
                   "Jane " + (char)0xD8 + " Clayton",
                System.Text.Encoding.UTF8);
                // Set destinations for the e-mail message.
                MailAddress to = new MailAddress("sneakyboo@gmail.com");
                // Specify the message content.
                using (MailMessage message = new MailMessage(from, to))
                {
                    message.Body = "This is a test e-mail message sent by an application. ";
                    // Include some non-ASCII characters in body and subject. 
                    string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
                    message.Body += Environment.NewLine + someArrows;
                    message.BodyEncoding = System.Text.Encoding.UTF8;
                    message.Subject = "test message 1" + someArrows;
                    message.SubjectEncoding = System.Text.Encoding.UTF8;
                    // Set the method that is called back when the send operation ends.

                    client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

                    // The userState can be any object that allows your callback  
                    // method to identify this send operation. 
                    // For this example, the userToken is a string constant. 
                    string userState = string.Format("{0}", token.ToString());
                    client.SendAsync(message, userState);
                    Monitor.Wait(locker);
                }
            }
        }


    }
}
