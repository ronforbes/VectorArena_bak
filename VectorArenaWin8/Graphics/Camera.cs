using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorArenaWin8
{
    class Camera
    {
        public Vector3 Position = Vector3.Backward;
        public Vector3 Target = Vector3.Zero;
        public Vector3 Up = Vector3.Up;
        public Rectangle ScreenDimensions;
        public Actor TargetActor;
        public Matrix World = Matrix.Identity;

        public Matrix View
        {
            get { return Matrix.CreateLookAt(Position, Target, Up); }
        }

        public Matrix Projection
        {
            get { return Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)ScreenDimensions.Width / (float)ScreenDimensions.Height, 1.0f, 1000.0f); }
        }

        public Camera(Rectangle screenDimensions)
        {
            ScreenDimensions = screenDimensions;
        }

        public void Update(GameTime gameTime)
        {
            if (TargetActor != null)
            {
                Position = new Vector3(TargetActor.Position.X, TargetActor.Position.Y, Position.Z);
                Target = TargetActor.Position;
            }
        }
    }
}
