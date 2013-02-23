using System;
using System.Collections.Concurrent;
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
        public ConcurrentDictionary<Direction, bool> Moving;
        public Player Player;

        public enum Direction
        {
            Left,
            Right,
            Forward,
            Backward
        }

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
            
            Moving = new ConcurrentDictionary<Direction, bool>();
            Moving.TryAdd(Direction.Left, false);
            Moving.TryAdd(Direction.Right, false);
            Moving.TryAdd(Direction.Forward, false);
            Moving.TryAdd(Direction.Backward, false);
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

            // Reset acceleration
            Acceleration = Vector2.Zero;

            // Apply rotation / acceleration based on movement
            if (Moving[Direction.Left])
            {
                Rotation += MathHelper.ToRadians(turnSpeed);
            }
            if (Moving[Direction.Right])
            {
                Rotation -= MathHelper.ToRadians(turnSpeed);
            }
            if (Moving[Direction.Forward])
            {
                Acceleration += new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)) * thrustAcceleration;
            }
            if (Moving[Direction.Backward])
            {
                Acceleration -= new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)) * thrustAcceleration;
            }

            if (fireTimer < fireDelay)
                fireTimer += fireSpeed;
        }
    }
}