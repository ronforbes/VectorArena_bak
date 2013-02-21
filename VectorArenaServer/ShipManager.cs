using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace VectorArenaServer
{
    public class ShipManager
    {
        public ConcurrentDictionary<string, Ship> Ships;

        public ShipManager()
        {
            Ships = new ConcurrentDictionary<string, Ship>();
        }

        public void Add(Ship ship)
        {
            Ships.TryAdd(ship.Player.ConnectionId, ship);
        }

        public void Update(TimeSpan elapsedTime)
        {
            Parallel.ForEach(Ships, ship =>
            {
                ship.Value.Update(elapsedTime);
            });
        }
    }
}