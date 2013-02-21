using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VectorArenaServer
{
    public class GameHub : Hub
    {
        Game game;

        public GameHub() : this(Game.Instance) { }

        public GameHub(Game game)
        {
            this.game = game;
        }

        public int AddPlayer()
        {
            int shipId = game.AddPlayer(Context.ConnectionId);

            Log("Added player: ConnectionId=" + Context.ConnectionId + ", shipId=" + shipId);

            return shipId;
        }

        public void Log(string message)
        {
            Clients.All.Log(message);
        }
    }
}