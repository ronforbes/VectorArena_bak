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

        public void StartMoving(string direction)
        {
            Ship ship = game.PlayerManager.Player(Context.ConnectionId).Ship;
            Ship.Direction shipDirection = (Ship.Direction)Enum.Parse(typeof(Ship.Direction), direction);
            ship.Moving[shipDirection] = true;
        }

        public void StopMoving(string direction)
        {
            Ship ship = game.PlayerManager.Player(Context.ConnectionId).Ship;
            Ship.Direction shipDirection = (Ship.Direction)Enum.Parse(typeof(Ship.Direction), direction);
            ship.Moving[shipDirection] = false;
        }
    }
}