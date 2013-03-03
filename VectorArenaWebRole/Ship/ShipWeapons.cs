using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VectorArenaWebRole
{
    public class ShipWeapons
    {
        readonly TimeSpan fireDelay = TimeSpan.FromMilliseconds(80);

        Ship ship;
        DateTime lastBulletFiredAt;
        BulletManager bulletManager;

        public ShipWeapons(Ship ship, BulletManager bulletManager)
        {
            lastBulletFiredAt = DateTime.Now;
            this.ship = ship;
            this.bulletManager = bulletManager;
        }

        public void Update()
        {
            // Fire based on the action being performed
            if (ship.Actions[Ship.Action.Fire])
            {
                if (DateTime.Now - lastBulletFiredAt >= fireDelay)
                {
                    Vector2 velocity = new Vector2((float)Math.Cos(ship.Movement.Rotation), (float)Math.Sin(ship.Movement.Rotation));
                    Vector2 position = new Vector2(ship.Movement.Position.X + velocity.X * ship.Radius * 2, ship.Movement.Position.Y + velocity.Y * ship.Radius * 2);
                    bulletManager.Add(new Bullet(position, velocity * Bullet.Speed, ship));
                    lastBulletFiredAt = DateTime.Now;
                }
            }
        }
    }
}