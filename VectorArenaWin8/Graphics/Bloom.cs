using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorArenaWin8.Graphics
{
    class Bloom : Postprocess
    {
        const float threshold = 0.1f;
        const float intensity = 2.0f;
        const int blurPasses = 4;

        static BlendState extractBrightColors = new BlendState
        {
            ColorSourceBlend = Blend.One,
            AlphaSourceBlend = Blend.One,

            ColorDestinationBlend = Blend.One,
            AlphaDestinationBlend = Blend.One,

            ColorBlendFunction = BlendFunction.Subtract,
            AlphaBlendFunction = BlendFunction.Subtract,
        };

        static BlendState additiveBlur = new BlendState
        {
            ColorSourceBlend = Blend.One,
            AlphaSourceBlend = Blend.One,

            ColorDestinationBlend = Blend.One,
            AlphaDestinationBlend = Blend.One,
        };

        static BlendState combineFinalResult = new BlendState
        {
            ColorSourceBlend = Blend.One,
            AlphaSourceBlend = Blend.One,

            ColorDestinationBlend = Blend.InverseSourceColor,
            AlphaDestinationBlend = Blend.InverseSourceColor,
        };

        GraphicsDevice graphicsDevice;
        SpriteBatch spriteBatch;
        RenderTarget2D halfSize;
        RenderTarget2D quarterSize;
        RenderTarget2D quarterSize2;

        public Bloom(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;

            PresentationParameters pp = this.graphicsDevice.PresentationParameters;

            int w = pp.BackBufferWidth;
            int h = pp.BackBufferHeight;

            halfSize = new RenderTarget2D(this.graphicsDevice, w / 2, h / 2, false, pp.BackBufferFormat, DepthFormat.None);
            quarterSize = new RenderTarget2D(this.graphicsDevice, w / 4, h / 4, false, pp.BackBufferFormat, DepthFormat.None);
            quarterSize2 = new RenderTarget2D(this.graphicsDevice, w / 4, h / 4, false, pp.BackBufferFormat, DepthFormat.None);
        }

        public override void Begin(RenderTarget2D scene)
        {
            // Shrink to half size.
            graphicsDevice.SetRenderTarget(halfSize);
            DrawSprite(scene, BlendState.Opaque);

            // Shrink again to quarter size, at the same time applying the threshold subtraction.
            graphicsDevice.SetRenderTarget(quarterSize);
            graphicsDevice.Clear(new Color(threshold, threshold, threshold));
            DrawSprite(halfSize, extractBrightColors);

            // Kawase blur filter (see http://developer.amd.com/media/gpu_assets/Oat-ScenePostprocessing.pdf)
            for (int i = 0; i < blurPasses; i++)
            {
                graphicsDevice.SetRenderTarget(quarterSize2);
                graphicsDevice.Clear(Color.Black);

                int w = quarterSize.Width;
                int h = quarterSize.Height;

                float brightness = 0.25f;

                // On the first pass, scale brightness to restore full range after the threshold subtraction.
                if (i == 0)
                    brightness /= (1 - threshold);

                // On the final pass, apply tweakable intensity adjustment.
                if (i == blurPasses - 1)
                    brightness *= intensity;

                Color tint = new Color(brightness, brightness, brightness);

                spriteBatch.Begin(0, additiveBlur);

                spriteBatch.Draw(quarterSize, new Vector2(0.5f, 0.5f), new Rectangle(i + 1, i + 1, w, h), tint);
                spriteBatch.Draw(quarterSize, new Vector2(0.5f, 0.5f), new Rectangle(-i, i + 1, w, h), tint);
                spriteBatch.Draw(quarterSize, new Vector2(0.5f, 0.5f), new Rectangle(i + 1, -i, w, h), tint);
                spriteBatch.Draw(quarterSize, new Vector2(0.5f, 0.5f), new Rectangle(-i, -i, w, h), tint);

                spriteBatch.End();

                Swap(ref quarterSize, ref quarterSize2);
            }
        }

        void DrawSprite(Texture2D source, BlendState blendState)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, blendState);
            spriteBatch.Draw(source, graphicsDevice.Viewport.Bounds, Color.White);
            spriteBatch.End();
        }

        public override void End(RenderTarget2D scene)
        {
            // Combine the original scene and bloom images.
            DrawSprite(scene, BlendState.Additive);
            DrawSprite(quarterSize, BlendState.Additive);
        }

        static void Swap<T>(ref T a, ref T b)
        {
            T tmp = a;
            a = b;
            b = tmp;
        }
    }
}
