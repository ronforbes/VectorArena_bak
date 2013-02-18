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
        public List<Bullet> Bullets;

        public BulletManager(int bulletCapacity)
            : base()
        {
            Bullets = new List<Bullet>(bulletCapacity);
        }

        public void CreateBullets()
        {
            for (int i = 0; i < Bullets.Capacity; i++)
            {
                Bullet b = new Bullet();
                Bullets.Add(b);
                AddChild(b);
            }
        }

        public void SpawnBullet(Vector2 position, Vector2 velocity)
        {
            for (int i = 0; i < Bullets.Capacity; i++)
            {
                if (!Bullets[i].Alive)
                {
                    Bullets[i].Spawn(position, velocity);
                    return;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(Camera camera)
        {
            LineBatch.Begin(Matrix.Identity, camera);

            foreach (Bullet b in Bullets)
            {
                b.Draw(LineBatch);
            }

            LineBatch.End();

            base.Draw(camera);
        }
    }
}
