using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace VectorArenaWebRole
{
    public class Ship
    {
        public int Id;
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public float Rotation;
        public float Radius;
        public ConcurrentDictionary<Action, bool> Actions;
        public Player Player;

        public enum Action
        {
            TurnLeft,
            TurnRight,
            ThrustForward,
            ThrustBackward,
            Fire
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
        BulletManager bulletManager;

        public Ship(Vector2 position, BulletManager bulletManager)
        {
            Id = Interlocked.Increment(ref idCounter);
            Position = position;
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
            Rotation = 0.0f;
            Radius = 15.0f;
            
            Actions = new ConcurrentDictionary<Action, bool>();
            Actions.TryAdd(Action.TurnLeft, false);
            Actions.TryAdd(Action.TurnRight, false);
            Actions.TryAdd(Action.ThrustForward, false);
            Actions.TryAdd(Action.ThrustBackward, false);
            Actions.TryAdd(Action.Fire, false);

            this.bulletManager = bulletManager;
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

            // Apply rotation / acceleration based on the action being performed
            if (Actions[Action.TurnLeft])
            {
                Rotation += MathHelper.ToRadians(turnSpeed);
            }
            if (Actions[Action.TurnRight])
            {
                Rotation -= MathHelper.ToRadians(turnSpeed);
            }
            if (Actions[Action.ThrustForward])
            {
                Acceleration += new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)) * thrustAcceleration;
            }
            if (Actions[Action.ThrustBackward])
            {
                Acceleration -= new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)) * thrustAcceleration;
            }

            // Fire based on the action being performed
            if (Actions[Action.Fire])
            {
                if (fireTimer >= fireDelay)
                {
                    Vector2 velocity = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
                    bulletManager.Add(new Bullet(new Vector2(Position.X + velocity.X * Radius * 2, Position.Y + velocity.Y * Radius * 2), velocity * Bullet.Speed, this));
                    fireTimer = 0.0f;
                }
            }

            if (fireTimer < fireDelay)
                fireTimer += fireSpeed;
        }
    }
}