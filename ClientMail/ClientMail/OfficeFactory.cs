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
using System.Globalization;

namespace ClientMail
{
    public class OfficeFactory:IDisposable
    {
        object locker = new object();
        public static bool mailSent = false;
        public static ConcurrentDictionary<string, string> _tokens;
        public static ConcurrentDictionary<string, string> _failure;
        private string _passWord;
        private string _userName;
        private int _port;
        private string _server;
        private string _subject;
        SmtpClient _client;

        public OfficeFactory(string server, int port, string userName, string password)
        {
            _server = server;
            _port = port;
            _userName = userName;
            _passWord = password;

            _client = new SmtpClient(_server, _port);
            // Credentials are necessary if the server requires the client  
            // to authenticate before it will send e-mail on the client's behalf.            
            _client.EnableSsl = true;
            _client.UseDefaultCredentials = false;
            _client.Credentials = new System.Net.NetworkCredential(_userName, _passWord);
            _client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
        }

        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            lock (locker)
            {
                // Get the unique identifier for this asynchronous operation.
                String token = (string)e.UserState;

                if (e.Cancelled)
                {
                    _failure.TryAdd(token + "-canceled", "Canceled");
                    Console.WriteLine("[{0}] Send canceled.", token);
                }
                if (!e.Cancelled && e.Error == null)
                {                   
                    _tokens.TryAdd(token, token);
                }

                if(e.Error!=null)
                {
                    //Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
                    _failure.TryAdd(token, e.Error.ToString());
                }

                mailSent = true;
                
                Monitor.Pulse(locker);
            }
            
        }
        public void SendCalendarEvent(string toRecipient, string subject, string body, DateTime when, object token)
        {
            lock (locker)
            {
                _subject = subject;
                if (_tokens == null) _tokens = new ConcurrentDictionary<string, string>();
                if (_failure == null) _failure = new ConcurrentDictionary<string, string>(); 

                // Command line argument must the the SMTP host.                           
                try
                {
                    // Specify the e-mail sender. 
                    // Create a mailing address that includes a UTF8 character 
                    // in the display name.
                    MailAddress from = new MailAddress("No_reply@backoffice.com", "No_Reply Next Action", Encoding.UTF8);
                    // Set destinations for the e-mail message.
                    MailAddress to = new MailAddress(toRecipient);
                    // Specify the message content.
                    using (MailMessage message = new MailMessage(from, to))
                    {
                        //message.CC.Add(new MailAddress("dmodiwirijo@hotmail.com"));

                        message.BodyEncoding = System.Text.Encoding.UTF8;
                        message.Subject = subject;
                        message.Body = body;
                        message.SubjectEncoding = System.Text.Encoding.UTF8;

                        DateTime appointment = when;
                        string filename = string.Format("next_action.{0}.ics", appointment.ToShortDateString());
                        var content = CreateContent(appointment, appointment, StyleIcs);
                        Attachment attach = new Attachment(content, filename, "text/plain");
                        message.Attachments.Add(attach);
                        //message.Attachments.Add(attach2);
                        // Set the method that is called back when the send operation ends.
                        

                        // The userState can be any object that allows your callback  
                        // method to identify this send operation. 
                        // For this example, the userToken is a string constant. 
                        string userState = string.Format("{0}", token.ToString());
                        _client.SendAsync(message, userState);
                        Monitor.Wait(locker);
                        //
                    }
                }
                finally
                {

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
        public MemoryStream CreateContent(DateTime startTime, DateTime endTime, Func<DateTime,DateTime, string[]> useStyle)
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
        public MemoryStream CreateContent(DateTime startTime, DateTime endTime)
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
        private string[] StyleIcs(DateTime startTime, DateTime endTime)
        {
            bool isDaylightSavingTime = TimeZoneInfo.Local.IsDaylightSavingTime(startTime);
            string schLocation = "";
            string schSubject = _subject;
            string schDescription = "";
            int hour = isDaylightSavingTime ? 9 : 8;
            DateTime schBeginDate = new DateTime(startTime.Year, startTime.Month, startTime.Day, hour ,0 ,0);
            DateTime schEndDate = new DateTime(endTime.Year, endTime.Month, endTime.Day, hour+1 ,0 ,0);
            string[] contents = {   "BEGIN      :VCALENDAR",
                                    "PRODID     :-//Meta Inc.//Meta//nl-NL",
                                    "BEGIN      : VEVENT",
                                    "DTSTART    :" + schBeginDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
                                    "DTEND      :" + schEndDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
                                    "LOCATION   :" + schLocation,
                                    "DESCRIPTION; ENCODING = QUOTED - PRINTABLE:" +schDescription,
                                    "SUMMARY    :" +schSubject, "PRIORITY: 3",
                                    "END        : VEVENT", "END: VCALENDAR" };
            return contents;
        }
        private string[] StyleIcal(DateTime startTime, DateTime endTime)
        {
            string schLocation = "";
            string schSubject = _subject;
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
                                    schBeginDate.ToLocalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
                                    schEndDate.ToLocalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
                                    schLocation,
                                    schDescription,
                                    "3",
                                    schSubject,
                                    "VEVENT",
                                    "VCALENDAR" };
            return contents;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).  
                    _client.SendCompleted -= SendCompletedCallback;
                    _client.Dispose();
                }                
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~OfficeFactory() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
