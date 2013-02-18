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

        List<Actor> actors;
        Bloom bloom;
        GraphicsDevice graphicsDevice;
        RenderTarget2D renderTarget;
        Color clearColor = new Color(0.0f, 0.0f, 0.1f, 0.1f);

        public Scene(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            Camera = new Camera(new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height));
            actors = new List<Actor>();            
            PresentationParameters parameters = graphicsDevice.PresentationParameters;
            renderTarget = new RenderTarget2D(graphicsDevice, parameters.BackBufferWidth, parameters.BackBufferHeight, false, parameters.BackBufferFormat, parameters.DepthStencilFormat, parameters.MultiSampleCount, RenderTargetUsage.DiscardContents);
        }

        public void AddActor(Actor actor)
        {
            actor.Scene = this;
            actors.Add(actor);
        }

        public void RemoveActor(Actor actor)
        {
            actors.Remove(actor);
        }

        public void LoadContent()
        {
            LineBatch = new LineBatch(graphicsDevice);
            PointBatch = new PointBatch(graphicsDevice);
            SpriteBatch = new SpriteBatch(graphicsDevice);
            bloom = new Bloom(graphicsDevice, SpriteBatch);

            foreach (Actor actor in actors)
                actor.LoadContent();
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (Actor actor in actors)
                actor.Update(gameTime);

            Camera.Update(gameTime);
        }

        public virtual void Draw()
        {
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.Clear(ClearOptions.Target, clearColor, 0, 0);

            foreach (Actor actor in actors)
                actor.Draw(Camera);

            bloom.Begin(renderTarget);

            graphicsDevice.SetRenderTarget(null);
            graphicsDevice.Clear(ClearOptions.Target, clearColor, 0, 0);

            bloom.End(renderTarget);
        }
    }
}
