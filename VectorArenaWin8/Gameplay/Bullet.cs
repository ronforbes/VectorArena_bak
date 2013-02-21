using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorArenaWin8
{
    class Bullet : Actor
    {
        public int Id;

        const float speed = 20.0f;
        const float lifeSpan = 100.0f;
        const float decay = 1.0f;
        const float length = 2.0f;
        const float width = 10.0f;

        float life;
        Color color = Color.White;

        public Bullet(Bullet bullet)
        {
            this.Position = bullet.Position;
            this.Velocity = bullet.Velocity;
            this.Acceleration = bullet.Acceleration;
        }

        public void Spawn(Vector2 position, Vector2 velocity)
        {
            Position = new Vector3(position, 0.0f);
            Velocity = new Vector3(velocity * speed, 0.0f);
            Alive = true;
            life = lifeSpan;
        }

        public void Die()
        {
            Alive = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (Alive)
            {
                Position += Velocity;

                life -= decay;
                if (life <= 0.0f)
                {
                    Die();
                }
            }
        }

        public override void Draw(Camera camera)
        {
            if (LineBatch == null || PointBatch == null)
            {
                LineBatch = Scene.LineBatch;
                PointBatch = Scene.PointBatch;
            }
            else
            {
                LineBatch.Begin(Matrix.Identity, camera);
                LineBatch.Draw(Position, Position - Velocity * length, width, color);
                LineBatch.End();
            }
        }
    }
}
