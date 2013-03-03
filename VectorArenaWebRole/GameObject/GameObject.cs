using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace VectorArenaWebRole
{
    public class GameObject
    {
        public int Id;
        public GameObjectMovement Movement;
        
        public float Radius;
        public Rectangle Bounds;
        public bool Disposed;

        public GameObject() 
        {
            Bounds = new Rectangle();
        }

        public bool IsCollidingWith(GameObject gameObject)
        {
            return Bounds.IntersectsWith(gameObject.Bounds);
        }

        public virtual void CollideWith(GameObject gameObject)
        {

        }

        public virtual void Update(TimeSpan elapsedTime)
        {
            Bounds.X = (int)Movement.Position.X;
            Bounds.Y = (int)Movement.Position.Y;
            Bounds.Width = 2 * (int)Radius;
            Bounds.Height = 2 * (int)Radius;
        }
    }
}