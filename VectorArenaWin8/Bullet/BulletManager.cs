using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorArenaWin8
{
    class BulletManager : GameObject
    {
        Dictionary<int, Bullet> bullets;

        public BulletManager()
            : base()
        {
            bullets = new Dictionary<int, Bullet>();
        }

        public void AddBullet(int id)
        {
            if (!bullets.ContainsKey(id))
            {
                bullets.Add(id, new Bullet(id));
                AddChild(bullets[id]);
            }
        }

        public void SyncBullets(List<Bullet> bullets)
        {
            foreach (Bullet bullet in bullets)
            {
                if (!this.bullets.ContainsKey(bullet.Id))
                {
                    AddBullet(bullet.Id);
                }

                this.bullets[bullet.Id].Position = bullet.Position;
                this.bullets[bullet.Id].Velocity = bullet.Velocity;
            }
        }
    }
}
