using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorArenaWin8.Graphics;

namespace VectorArenaWin8
{
    class Scene
    {
        public LineBatch LineBatch;
        public PointBatch PointBatch;
        public SpriteBatch SpriteBatch;
        public Camera Camera;

        List<GameObject> gameObjects;
        Bloom bloom;
        GraphicsDevice graphicsDevice;
        RenderTarget2D renderTarget;
        Color clearColor = new Color(0.0f, 0.0f, 0.1f, 0.1f);

        public Scene(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            Camera = new Camera(new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height));
            gameObjects = new List<GameObject>();            
            PresentationParameters parameters = graphicsDevice.PresentationParameters;
            renderTarget = new RenderTarget2D(graphicsDevice, parameters.BackBufferWidth, parameters.BackBufferHeight, false, parameters.BackBufferFormat, parameters.DepthStencilFormat, parameters.MultiSampleCount, RenderTargetUsage.DiscardContents);
        }

        public void AddActor(GameObject gameObject)
        {
            gameObject.Scene = this;
            gameObjects.Add(gameObject);
        }

        public void RemoveActor(GameObject gameObject)
        {
            gameObjects.Remove(gameObject);
        }

        public void LoadContent()
        {
            LineBatch = new LineBatch(graphicsDevice);
            PointBatch = new PointBatch(graphicsDevice);
            SpriteBatch = new SpriteBatch(graphicsDevice);
            bloom = new Bloom(graphicsDevice, SpriteBatch);

            foreach (GameObject gameObject in gameObjects)
                gameObject.LoadContent();
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (GameObject gameObject in gameObjects)
                gameObject.Update(gameTime);

            Camera.Update(gameTime);
        }

        public virtual void Draw()
        {
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.Clear(ClearOptions.Target, clearColor, 0, 0);

            foreach (GameObject gameObject in gameObjects)
                gameObject.Draw(Camera);

            bloom.Begin(renderTarget);

            graphicsDevice.SetRenderTarget(null);
            graphicsDevice.Clear(ClearOptions.Target, clearColor, 0, 0);

            bloom.End(renderTarget);
        }
    }
}
