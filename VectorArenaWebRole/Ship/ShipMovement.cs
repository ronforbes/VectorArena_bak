using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VectorArenaWebRole
{
    public class ShipMovement : GameObjectMovement
    {
        const float thrustAcceleration = 500.0f;
        const float maxSpeed = 2000.0f;
        const float dragCoefficient = 0.95f;
        const float turnSpeed = 3.0f;
        const float maxHealth = 1000;

        Ship ship;

        public ShipMovement(Ship ship, Vector2 position)
        {
            this.ship = ship;
            Position = position;
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
            Rotation = 0.0f;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            // Update the ship's velocity based on acceleration
            Velocity += Acceleration * (float)elapsedTime.TotalSeconds;

            // Cap max speed
            if (Velocity.Length() > maxSpeed)
            {
                Velocity.Normalize();
                Velocity *= maxSpeed;
            }

            // Apply drag to velocity
            Velocity *= dragCoefficient;

            // Update the ship's position based on velocity
            Position += Velocity * (float)elapsedTime.TotalSeconds;

            // Reset acceleration
            Acceleration = Vector2.Zero;

            // Apply rotation / acceleration based on the action being performed
            if (ship.Actions[Ship.Action.TurnLeft])
            {
                Rotation += MathHelper.ToRadians(turnSpeed);
            }
            if (ship.Actions[Ship.Action.TurnRight])
            {
                Rotation -= MathHelper.ToRadians(turnSpeed);
            }
            if (ship.Actions[Ship.Action.ThrustForward])
            {
                Acceleration += new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)) * thrustAcceleration;
            }
            if (ship.Actions[Ship.Action.ThrustBackward])
            {
                Acceleration -= new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)) * thrustAcceleration;
            }
        }
    }
}