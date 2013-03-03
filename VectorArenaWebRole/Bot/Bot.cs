using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace VectorArenaWebRole
{
    public class Bot : GameObject
    {
        public bool Alive;

        const float thrustAcceleration = 100.0f;
        const float maxSpeed = 400.0f;
        const float dragCoefficient = 0.95f;
        const float lineWidth = 5.0f;
        const float lightRadius = 25.0f;

        static int idCounter = 0;

        public Bot(Vector2 position)
            : base()
        {
            Id = Interlocked.Increment(ref idCounter);
            Movement.Position = position;
            Movement.Velocity = Vector2.Zero;
            Alive = true;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            // Update the bot's velocity based on acceleration
            Movement.Velocity += Movement.Acceleration * (float)elapsedTime.TotalSeconds;

            // Cap max speed
            if (Movement.Velocity.Length() > maxSpeed)
            {
                Movement.Velocity.Normalize();
                Movement.Velocity *= maxSpeed;
            }

            // Apply drag to velocity
            Movement.Velocity *= dragCoefficient;

            // Update the bot's position based on velocity
            Movement.Position += Movement.Velocity * (float)elapsedTime.TotalSeconds;

            // Reset acceleration
            Movement.Acceleration = Vector2.Zero;

            base.Update(elapsedTime);
        }
    }
}