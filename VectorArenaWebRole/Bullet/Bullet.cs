using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace VectorArenaWebRole
{
    public class Bullet
    {
        public const float Speed = 500.0f;

        public int Id;
        public Vector2 Position;
        public Vector2 Velocity;
        public Ship Ship;

        static int idCounter = 0;

        public Bullet(Vector2 position, Vector2 velocity, Ship ship)
        {
            Position = position;
            Velocity = velocity;
            Id = Interlocked.Increment(ref idCounter);
            Ship = ship;
        }

        public void Update(TimeSpan elapsedTime)
        {
            Position += Velocity * elapsedTime.TotalSeconds;
        }
    }
}