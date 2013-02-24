using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VectorArenaWebRole
{
    public class Player
    {
        public string ConnectionId;
        public Ship Ship;

        public Player(string connectionId, Ship ship)
        {
            ConnectionId = connectionId;
            Ship = ship;

            if (ship != null)
            {
                ship.Player = this;
            }
        }

        public void Sync(object[] gameState, IHubContext hubContext)
        {
            hubContext.Clients.Client(ConnectionId).Sync(gameState);
        }
    }
}