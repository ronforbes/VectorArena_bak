using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorArenaWin8
{
    class BulletManager : Actor
    {
        Dictionary<int, Bullet> bullets;

        public BulletManager()
            : base()
        {
            bullets = new Dictionary<int, Bullet>();
        }

        public void AddBullet(Bullet bullet)
        {
            if (!bullets.ContainsKey(bullet.Id))
            {
                bullets.Add(bullet.Id, new Bullet(bullet));
                AddChild(bullets[bullet.Id]);
            }
        }

        public void SyncBullets(List<Bullet> bullets)
        {

        }
    }
}
