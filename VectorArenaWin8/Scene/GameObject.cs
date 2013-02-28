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
    class GameObject
    {
        public int Id;
        public Vector3 Position;
        public Vector3 Velocity;
        public Vector3 Acceleration;
        public float Rotation;
        public float Radius;
        public LineBatch LineBatch;
        public PointBatch PointBatch;
        public SpriteBatch SpriteBatch;
        public Scene Scene;
        public GameObject Parent;
        public List<GameObject> Children;

        public GameObject()
        {
            Children = new List<GameObject>();
        }

        public void AddChild(GameObject child)
        {
            child.Parent = this;
            child.Scene = Scene;
            lock (Children)
            {
                Children.Add(child);
            }
        }

        public void RemoveChild(GameObject child)
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

            foreach(GameObject gameObject in Children)
            {
                gameObject.LoadContent();
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach(GameObject gameObject in Children)
                gameObject.Update(gameTime);
        }

        public virtual void Draw(Camera camera)
        {
            lock (Children)
            {
                foreach (GameObject gameObject in Children)
                {
                    gameObject.Draw(camera);
                }
            }
        }
    }
}
