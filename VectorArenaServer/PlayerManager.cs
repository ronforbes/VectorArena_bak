using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VectorArenaServer
{
    public class PlayerManager
    {
        public ConcurrentDictionary<string, Player> Players;

        public PlayerManager()
        {
            Players = new ConcurrentDictionary<string, Player>();
        }

        public void Add(Player player)
        {
            Players.TryAdd(player.ConnectionId, player);
        }

        public Player Player(string connectionId)
        {
            Player player;

            Players.TryGetValue(connectionId, out player);

            return player;
        }
    }
}