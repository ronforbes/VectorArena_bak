using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VectorArenaWebRole
{
    public class BulletManager
    {
        public List<Bullet> Bullets;

        CollisionManager collisionManager;

        public BulletManager(CollisionManager collisionManager)
        {
            Bullets = new List<Bullet>();
            this.collisionManager = collisionManager;
        }

        public void Add(Bullet bullet)
        {
            lock (Bullets)
            {
                Bullets.Add(bullet);
                collisionManager.Add(bullet);
            }
        }

        public void Update(TimeSpan elapsedTime)
        {
            lock (Bullets)
            {
                foreach(Bullet bullet in Bullets)
                {
                    bullet.Update(elapsedTime);
                }

                Bullets.RemoveAll(b => b.Disposed);
            }
        }
    }
}