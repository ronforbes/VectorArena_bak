using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace VectorArenaWebRole
{
    public class ShipManager
    {
        public ConcurrentDictionary<string, Ship> Ships;

        ConcurrentDictionary<Ship, DateTime> shipsToRespawn;
        CollisionManager collisionManager;

        public ShipManager(CollisionManager collisionManager)
        {
            Ships = new ConcurrentDictionary<string, Ship>();
            this.collisionManager = collisionManager;
        }

        public void Add(Ship ship)
        {
            if (Ships.TryAdd(ship.Player.ConnectionId, ship))
            {
                collisionManager.Add(ship);
            }
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