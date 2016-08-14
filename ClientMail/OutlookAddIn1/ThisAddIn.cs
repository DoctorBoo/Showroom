using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;

namespace OutlookAddIn1
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            // Note: Outlook no longer raises this event. If you have code that 
            //    must run when Outlook shuts down, see http://go.microsoft.com/fwlink/?LinkId=506785
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        public void AddAppointment()
        {
            try
            {
                AppointmentItem newAppointment =
                    (AppointmentItem)
                this.Application.CreateItem(OlItemType.olAppointmentItem);
                newAppointment.Start = DateTime.Now.AddHours(2);
                newAppointment.End = DateTime.Now.AddHours(3);
                newAppointment.Location = "ConferenceRoom #2345";
                newAppointment.Body =
                    "We will discuss progress on the group project.";
                newAppointment.AllDayEvent = false;
                newAppointment.Subject = "Group Project";
                newAppointment.Recipients.Add("Roger Harui");
                Recipients sentTo = newAppointment.Recipients;
                Recipient sentInvite = null;
                sentInvite = sentTo.Add("Holly Holt");
                sentInvite.Type = (int)Microsoft.Office.Interop.Outlook.OlMeetingRecipientType
                    .olRequired;
                sentInvite = sentTo.Add("David Junca ");
                sentInvite.Type = (int)Microsoft.Office.Interop.Outlook.OlMeetingRecipientType
                    .olOptional;
                sentTo.ResolveAll();
                newAppointment.Save();
                newAppointment.Display(true);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("The following error occurred: " + ex.Message);
            }
        }
        public void AddMeeting(object sender, System.EventArgs e)
        {
            AppointmentItem agendaMeeting = (AppointmentItem)
                this.Application.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.
                olAppointmentItem);

            if (agendaMeeting != null)
            {
                agendaMeeting.MeetingStatus =
                    Microsoft.Office.Interop.Outlook.OlMeetingStatus.olMeeting;
                agendaMeeting.Location = "Conference Room";
                agendaMeeting.Subject = "Discussing the Agenda";
                agendaMeeting.Body = "Let's discuss the agenda.";
                agendaMeeting.Start = new DateTime(2005, 5, 5, 5, 0, 0);
                agendaMeeting.Duration = 60;
                Recipient recipient =
                    agendaMeeting.Recipients.Add("Nate Sun");
                recipient.Type =
                    (int)OlMeetingRecipientType.olRequired;
                ((_AppointmentItem)agendaMeeting).Send();
            }
        }
        #endregion
    }
}
