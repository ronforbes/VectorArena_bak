using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorArenaWin8
{
    class Ship : Actor
    {
        public int Id;
        public Dictionary<Action, bool> Actions;

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

        float fireTimer = 0.0f;
        List<Vector3> vertices;
        Color color = Color.White;
        Color lightColor = new Color(0.25f, 0.25f, 0.25f, 1.0f);

        public Ship(int id) : base()
        {
            Id = id;
            
            Actions = new Dictionary<Action, bool>();
            Actions.Add(Action.TurnLeft, false);
            Actions.Add(Action.TurnRight, false);
            Actions.Add(Action.ThrustForward, false);
            Actions.Add(Action.ThrustBackward, false);
            Actions.Add(Action.Fire, false);

            Radius = 15.0f;

            vertices = new List<Vector3>();
            vertices.Add(new Vector3(15.0f, 0.0f, 0.0f));
            vertices.Add(new Vector3(-15.0f, -15.0f, 0.0f));
            vertices.Add(new Vector3(-10.0f, 0.0f, 0.0f));
            vertices.Add(new Vector3(-15.0f, 15.0f, 0.0f));
        }

        public void Fire()
        {
            if (fireTimer >= fireDelay)
            {
                GameplayScene scene = Scene as GameplayScene;
                Vector2 velocity = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
                //scene.BulletManager.SpawnBullet(new Vector2(Position.X + velocity.X * Radius * 2, Position.Y + velocity.Y * Radius * 2), velocity);
                fireTimer = 0.0f;
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Update the ship's velocity based on acceleration
            Velocity += Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Cap max speed
            if (Velocity.Length() > maxSpeed)
            {
                Velocity.Normalize();
                Velocity *= maxSpeed;
            }

            // Apply drag to velocity
            Velocity *= dragCoefficient;

            // Update the ship's position based on velocity
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Reset acceleration
            Acceleration = Vector3.Zero;

            // Apply rotation / acceleration based on movement
            if (Actions[Action.TurnLeft])
            {
                Rotation += MathHelper.ToRadians(turnSpeed);
            }
            if (Actions[Action.TurnRight])
            {
                Rotation -= MathHelper.ToRadians(turnSpeed);
            }
            if(Actions[Action.ThrustForward])
            {
                Acceleration += new Vector3((float)Math.Cos(Rotation), (float)Math.Sin(Rotation), 0.0f) * thrustAcceleration;
            }
            if(Actions[Action.ThrustBackward])
            {
                Acceleration -= new Vector3((float)Math.Cos(Rotation), (float)Math.Sin(Rotation), 0.0f) * thrustAcceleration;
            }

            if(fireTimer < fireDelay)
                fireTimer += fireSpeed;
        }

        public override void Draw(Camera camera)
        {
            // Draw the ship
            Matrix rotation = Matrix.CreateRotationZ(Rotation);
            Matrix translation = Matrix.CreateTranslation(Position);

            if (LineBatch == null || PointBatch == null)
            {
                LineBatch = Scene.LineBatch;
                PointBatch = Scene.PointBatch;
            }
            else
            {
                LineBatch.Begin(rotation * translation, camera);

                for (int v = 0; v < vertices.Count; v++)
                {
                    LineBatch.Draw(vertices[v], vertices[(v + 1) % vertices.Count], lineWidth, color);
                }

                LineBatch.End();

                PointBatch.Begin(translation, camera);
                PointBatch.Draw(Vector3.Zero, lightRadius, lightColor);
                PointBatch.End();
            }
        }
    }
}
