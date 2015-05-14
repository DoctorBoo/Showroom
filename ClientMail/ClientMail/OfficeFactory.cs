using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.ComponentModel;
using System.Net;
using System.Collections.Concurrent;
using System.Threading;
using System.IO;

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
                MailAddress from = new MailAddress("No_reply@backoffice.com",
                   "No_reply " + (char)0xD8 + " sygnion", Encoding.UTF8);
                // Set destinations for the e-mail message.
                MailAddress to = new MailAddress("sneakyboo@gmail.com");
                // Specify the message content.
                using (MailMessage message = new MailMessage(from, to))
                {
                    message.CC.Add(new MailAddress("dmodiwirijo@hotmail.com"));
                    //message.CC.Add(new MailAddress("sneakyboo@gmail.com"));
                    message.Body = "This is a test e-mail message sent by an application. ";
                    // Include some non-ASCII characters in body and subject. 
                    string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
                    message.Body += Environment.NewLine + someArrows;
                    message.BodyEncoding = System.Text.Encoding.UTF8;
                    message.Subject = "NEXT ACTION";
                    message.SubjectEncoding = System.Text.Encoding.UTF8;

                    DateTime appointment = new DateTime(2015, 7, 7);
                    string filename = string.Format("next_action.{0}.ics", appointment.ToShortDateString());                    
                    Attachment attach = new Attachment(CreateContent(appointment, appointment, StyleIcs), filename, "text/plain");                    
                    message.Attachments.Add(attach);
                    //message.Attachments.Add(attach2);
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
        public static MemoryStream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        public static MemoryStream CreateContent(DateTime startTime, DateTime endTime, Func<DateTime,DateTime, string[]> useStyle)
        {
            //CalendarReader calendar;
            MemoryStream stream = new MemoryStream();
            try
            {
                string[] contents = null;
                if (useStyle == null)
                    contents = StyleIcs(startTime, endTime);
                else
                    contents = useStyle(startTime, endTime);
                String concatenated = String.Join("\r\n", contents);
                stream = GenerateStreamFromString(concatenated);

            }
            catch (Exception)
            {
                stream.Dispose();
                throw;
            }
            return stream;
        }
        public static MemoryStream CreateContent(DateTime startTime, DateTime endTime)
        {
            //CalendarReader calendar;
            MemoryStream stream = new MemoryStream();
            try
            {
                string[] contents = StyleIcs(startTime, endTime);
                String concatenated = String.Join("\r\n", contents);
                stream = GenerateStreamFromString(concatenated);

            }
            catch (Exception)
            {
                stream.Dispose();
                throw;
            }
            return stream;
        }
        private static string[] StyleIcs(DateTime startTime, DateTime endTime)
        {
            string schLocation = "";
            string schSubject = "NEXT ACTION";
            string schDescription = "";
            DateTime schBeginDate = Convert.ToDateTime(startTime);
            DateTime schEndDate = Convert.ToDateTime(endTime);
            string[] contents = {   "BEGIN      :VCALENDAR",
                                    "PRODID     :-//Flo Inc.//FloSoft//EN",
                                    "BEGIN      : VEVENT",
                                    "DTSTART    :" +schBeginDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
                                    "DTEND      :" +schEndDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
                                    "LOCATION   :" +schLocation,
                                    "DESCRIPTION; ENCODING = QUOTED - PRINTABLE:" +schDescription,
                                    "SUMMARY    :" +schSubject, "PRIORITY: 3",
                                    "END        : VEVENT", "END: VCALENDAR" };
            return contents;
        }
        private static string[] StyleIcal(DateTime startTime, DateTime endTime)
        {
            string schLocation = "";
            string schSubject = "NEXT ACTION";
            string schDescription = "";
            DateTime schBeginDate = Convert.ToDateTime(startTime);
            DateTime schEndDate = Convert.ToDateTime(endTime);
            string[] contents = {   "BEGIN                                      ",
                                    "PRODID                                     ",
                                    "BEGIN                                      ",
                                    "DTSTART                                    ",
                                    "DTEND                                      ",
                                    "LOCATION                                   ",
                                    "DESCRIPTION",
                                    "SUMMARY                                    ",
                                    "PRIORITY                                   ",
                                    "END                                        ",
                                    "END                                        ",
                                    "VCALENDAR",
                                    "-//Flo Inc.//FloSoft//EN",
                                    " VEVENT",
                                    schBeginDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
                                    schEndDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
                                    schLocation,
                                    schDescription,
                                    "3",
                                    schSubject,
                                    "VEVENT",
                                    "VCALENDAR" };
            return contents;
        }
    }
}
