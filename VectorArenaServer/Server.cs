using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VectorArenaServer
{
    public class Server : Hub
    {
        public void Send(string message)
        {
            Clients.All.addMessage(message);
        }
    }
}