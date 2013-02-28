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
        const float lifeSpan = 100.0f;
        const float decay = 1.0f;
        const float radius = 10.0f;

        Color color = Color.White;

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
            if (PointBatch == null)
            {
                PointBatch = Scene.PointBatch;
            }
            
            PointBatch.Begin(Matrix.Identity, camera);
            PointBatch.Draw(Position, radius, color);
            PointBatch.End();
        }
    }
}
