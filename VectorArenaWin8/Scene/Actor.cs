using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorArenaWin8
{
    // An object in the game that can be updated and drawn
    class Actor
    {
        public bool Alive;
        public Vector3 Position;
        public Vector3 Velocity;
        public Vector3 Acceleration;
        public float Rotation;
        public float Radius;
        public LineBatch LineBatch;
        public PointBatch PointBatch;
        public SpriteBatch SpriteBatch;
        public Scene Scene;
        public Actor Parent;
        public List<Actor> Children;

        public Actor()
        {
            Children = new List<Actor>();
        }

        public void AddChild(Actor child)
        {
            child.Parent = this;
            child.Scene = Scene;
            lock (Children)
            {
                Children.Add(child);
            }
        }

        public void RemoveChild(Actor child)
        {
            lock (Children)
            {
                Children.Remove(child);
            }
        }

        public virtual void LoadContent()
        {
            LineBatch = Scene.LineBatch;
            PointBatch = Scene.PointBatch;
            SpriteBatch = Scene.SpriteBatch;

            foreach(Actor actor in Children)
            {
                actor.LoadContent();
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach(Actor actor in Children)
                actor.Update(gameTime);
        }

        public virtual void Draw(Camera camera)
        {
            lock (Children)
            {
                foreach (Actor actor in Children)
                {
                    actor.Draw(camera);
                }
            }
        }
    }
}
