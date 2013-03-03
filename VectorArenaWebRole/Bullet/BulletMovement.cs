using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VectorArenaWebRole
{
    public class BulletMovement : GameObjectMovement
    {
        public BulletMovement(Vector2 position, Vector2 velocity)
        {
            Position = position;
            Velocity = velocity;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            Position += Velocity * (float)elapsedTime.TotalSeconds;
        }
    }
}