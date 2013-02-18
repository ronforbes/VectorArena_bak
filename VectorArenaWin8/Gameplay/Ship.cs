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
        const float maxSpeed = 10.0f;
        const float turnSpeed = 3.0f;
        const float thrustAcceleration = 1.0f;
        const float fireSpeed = 1.0f;
        const float fireDelay = 5.0f;
        const float lineWidth = 5.0f;
        const float lightRadius = 50.0f;

        float fireTimer = 0.0f;
        List<Vector3> vertices;
        Color color = Color.White;
        Color lightColor = new Color(0.25f, 0.25f, 0.25f, 1.0f);

        public Ship() : base()
        {
            Radius = 15.0f;
        }

        public void Turn(int direction)
        {
            Rotation += MathHelper.ToRadians(turnSpeed) * direction;
        }

        public void Thrust(int direction)
        {
            Acceleration = new Vector3((float)Math.Cos(Rotation), (float)Math.Sin(Rotation), 0.0f) * thrustAcceleration * direction;
        }

        public void Fire()
        {
            if (fireTimer >= fireDelay)
            {
                GameplayScene scene = Scene as GameplayScene;
                Vector2 velocity = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
                scene.BulletManager.SpawnBullet(new Vector2(Position.X + velocity.X * Radius * 2, Position.Y + velocity.Y * Radius * 2), velocity);
                fireTimer = 0.0f;
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Update the ship's velocity and position
            Velocity += Acceleration;
            if (Velocity.Length() > maxSpeed)
            {
                Velocity.Normalize();
                Velocity *= maxSpeed;
            }

            Velocity *= 0.95f;

            Position += Velocity;

            if(fireTimer < fireDelay)
                fireTimer += fireSpeed;
        }

        public override void Draw(Camera camera)
        {
            // Draw the ship
            Matrix rotation = Matrix.CreateRotationZ(Rotation);
            Matrix translation = Matrix.CreateTranslation(Position);

            LineBatch.Begin(rotation * translation, camera);

            LineBatch.Draw(new Vector3(-15.0f, 15.0f, 0.0f), new Vector3(15.0f, 15.0f, 0.0f), lineWidth, color);
            LineBatch.Draw(new Vector3(15.0f, 15.0f, 0.0f), new Vector3(15.0f, -15.0f, 0.0f), lineWidth, color);
            LineBatch.Draw(new Vector3(15.0f, -15.0f, 0.0f), new Vector3(-15.0f, -15.0f, 0.0f), lineWidth, color);
            LineBatch.Draw(new Vector3(-15.0f, -15.0f, 0.0f), new Vector3(-15.0f, 15.0f, 0.0f), lineWidth, color);

            LineBatch.End();

            PointBatch.Begin(translation, camera);
            PointBatch.Draw(Vector3.Zero, lightRadius, lightColor);
            PointBatch.End();
        }
    }
}
