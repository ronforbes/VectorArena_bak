using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Web;

namespace VectorArenaWebRole
{
    public class Bullet : GameObject
    {
        public const float Speed = 500.0f;
        public readonly TimeSpan LifeSpan = TimeSpan.FromSeconds(1);
        public Ship Ship;

        static int idCounter = 0;

        DateTime createdAt;
        
        public Bullet(Vector2 position, Vector2 velocity, Ship ship) : base()
        {
            Id = Interlocked.Increment(ref idCounter);
            Movement = new BulletMovement(position, velocity);
            Radius = 10.0f;
            Ship = ship;

            createdAt = DateTime.Now;
        }

        public void Die()
        {
            Disposed = true;
        }

        public override void CollideWith(GameObject gameObject)
        {
            if (gameObject is Ship)
            {
                // Ignore collision with the bullet's own ship
                if (gameObject != Ship)
                {
                    (gameObject as Ship).Damage(100);
                }
            }

            if (gameObject is Bullet)
            {
                (gameObject as Bullet).Die();
            }

            base.CollideWith(gameObject);
        }
        
        public override void Update(TimeSpan elapsedTime)
        {
            Movement.Update(elapsedTime);

            // Dispose of the bullet after its life span has expired
            if (DateTime.Now - createdAt >= LifeSpan)
            {
                Disposed = true;
            }

            base.Update(elapsedTime);
        }
    }
}