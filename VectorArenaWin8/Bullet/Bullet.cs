using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorArenaWin8
{
    class Bullet : GameObject
    {
        const float length = 10.0f;
        const float width = 5.0f;
        const float radius = 20.0f;

        Color color = Color.White;
        Color lightColor = new Color(0.25f, 0.25f, 0.25f, 1.0f);

        public Bullet(int id) : base()
        {
            Id = id;
        }

        public override void Update(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(Camera camera)
        {
            if (LineBatch == null || PointBatch == null)
            {
                LineBatch = Scene.LineBatch;
                PointBatch = Scene.PointBatch;
            }

            LineBatch.Begin(Matrix.Identity, camera);
            LineBatch.Draw(Position, Position - Velocity / Velocity.Length() * length, width, color);
            LineBatch.End();

            PointBatch.Begin(Matrix.Identity, camera);
            PointBatch.Draw(Position, radius, lightColor);
            PointBatch.End();
        }
    }
}
