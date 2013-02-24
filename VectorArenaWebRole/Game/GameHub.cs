using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VectorArenaWebRole
{
    public class GameHub : Hub
    {
        Game game;

        public GameHub() : this(Game.Instance) { }

        public GameHub(Game game)
        {
            this.game = game;
        }

        public void Log(string message)
        {
            Clients.All.Log(message);
        }

        public int AddPlayer()
        {
            int shipId = game.AddPlayer(Context.ConnectionId);

            Log("Added player: ConnectionId=" + Context.ConnectionId + ", shipId=" + shipId);

            return shipId;
        }

        public void StartAction(string action)
        {
            Ship ship = game.PlayerManager.Player(Context.ConnectionId).Ship;
            Ship.Action shipAction = (Ship.Action)Enum.Parse(typeof(Ship.Action), action);
            ship.Actions[shipAction] = true;
        }

        public void StopAction(string action)
        {
            Ship ship = game.PlayerManager.Player(Context.ConnectionId).Ship;
            Ship.Action shipAction = (Ship.Action)Enum.Parse(typeof(Ship.Action), action);
            ship.Actions[shipAction] = false;
        }
    }
}