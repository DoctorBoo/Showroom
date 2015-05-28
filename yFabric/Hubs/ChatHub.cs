using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using yFabric.Counters;

namespace yFabric.Hubs
{
    public class ChatHub : Hub
    {
        public ChatHub()
        {

        }

        private const string _message = "{0}";

        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, string.Format(_message, Context.User.Identity.Name ,message));

        }
        public void GetIdentity()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                Clients.Caller.Identity(Context.User.Identity.Name);
            }
            else
            {
                Clients.Caller.Identity(string.Format(_message, "User-" + MvcApplication.TicketNr));
            }
            
        }
        public void SendMessage(string message)
        {
            Clients.All.newMessage(string.Format(_message, message));
        }
    }
}