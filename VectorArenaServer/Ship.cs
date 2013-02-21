using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace VectorArenaServer
{
    public class Ship
    {
        public int Id;
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public float Rotation;
        public Player Player;

        const float thrustAcceleration = 500.0f;
        const float maxSpeed = 2000.0f;
        const float dragCoefficient = 0.95f;
        const float turnSpeed = 3.0f;
        const float fireSpeed = 1.0f;
        const float fireDelay = 5.0f;
        const float lineWidth = 5.0f;
        const float lightRadius = 50.0f;

        static int idCounter = 0;

        float fireTimer = 0.0f;

        public Ship(Vector2 position)
        {
            Id = Interlocked.Increment(ref idCounter);
            Position = position;
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
            Rotation = 0.0f;
        }

        public void Update(TimeSpan elapsedTime)
        {
            // Update the ship's velocity based on acceleration
            Velocity += Acceleration * elapsedTime.TotalSeconds;

            // Cap max speed
            if (Velocity.Length() > maxSpeed)
            {
                Velocity.Normalize();
                Velocity *= maxSpeed;
            }

            // Apply drag to velocity
            Velocity *= dragCoefficient;

            // Update the ship's position based on velocity
            Position += Velocity * elapsedTime.TotalSeconds;

            if (fireTimer < fireDelay)
                fireTimer += fireSpeed;
        }
    }
}