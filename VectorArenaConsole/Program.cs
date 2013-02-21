using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace VectorArenaConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Connect to the server
            HubConnection hubConnection = new HubConnection("http://localhost:2697");

            // Create a proxy to the server
            IHubProxy proxy = hubConnection.CreateHubProxy("gameHub");

            // Print the message when it comes in
            proxy.On("Log", data => Console.WriteLine(data));

            proxy.On("Sync", data => Console.WriteLine(data));

            // Start the connection
            hubConnection.Start().Wait();

            string line = null;
            while ((line = Console.ReadLine()) != null)
            {
                // Send a message to the server
                proxy.Invoke("Send", line).Wait();
            }
        }
    }
}
