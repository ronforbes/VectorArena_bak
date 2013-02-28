using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VectorArenaWebRole
{
    public class BulletManager
    {
        public List<Bullet> Bullets;

        public BulletManager()
        {
            Bullets = new List<Bullet>();
        }

        public void Add(Bullet bullet)
        {
            lock (Bullets)
            {
                Bullets.Add(bullet);
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