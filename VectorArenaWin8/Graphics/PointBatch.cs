using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorArenaWin8
{
    // Draws points in an efficient batched manner
    class PointBatch : IDisposable
    {
        // Sizes of the vertex and index buffers
        const int vertexBufferSize = 400;
        const int indexBufferSize = 600;

        // Points are composed of 4 vertices and 6 indices specified as a triangle list
        const int verticesPerPoint = 4;
        const int indicesPerPoint = 6;

        // Vertex and index buffers
        VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[vertexBufferSize];
        short[] indices = new short[indexBufferSize];

        // Position of the next vertex and index to be placed in the buffers
        int vertexBufferPosition = 0;
        int indexBufferPosition = 0;

        // Effect containing the shader used to draw points
        BasicEffect effect;

        // Texture to be drawn on points
        Texture2D texture;

        // Graphics device used to draw points
        GraphicsDevice graphicsDevice;

        // Ensures that Begin is called before End
        bool hasBegun = false;

        bool isDisposed = false;

        // Initializes a new point batch
        public PointBatch(GraphicsDevice graphicsDevice)
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException("graphicsDevice");

            this.graphicsDevice = graphicsDevice;

            // Sets up the point texture
            int radius = 256;
            int length = radius * 2;
            Color[] colors = new Color[length * length];

            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    float distance = Vector2.Distance(Vector2.One, new Vector2(x, y) / radius);
                    float falloff = MathHelper.Clamp(Falloff(distance, radius), 0.0f, 1.0f);
                    colors[y * length + x] = new Color(1.0f, 1.0f, 1.0f, falloff);
                }
            }

            texture = new Texture2D(graphicsDevice, length, length);
            texture.SetData(colors);

            // Sets up the effect
            effect = new BasicEffect(graphicsDevice);
            effect.VertexColorEnabled = true;
            effect.TextureEnabled = true;
            effect.Texture = texture;
        }

        float Falloff(float distance, int radius)
        {
            float falloff = 0.0f;
            if (distance >= 0 && distance <= radius / 3)
            {
                falloff = 1.0f - 3.0f * (float)Math.Pow(distance, 2);
            }
            if (distance > radius / 3 && distance <= radius)
            {
                falloff = 1.5f * (float)Math.Pow(1.0f - distance, 2);
            }

            return falloff;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !isDisposed)
            {
                if (effect != null)
                    effect.Dispose();

                isDisposed = true;
            }
        }

        // Prepares points to be drawn
        public void Begin(Matrix world, Camera camera)
        {
            if (hasBegun)
                throw new InvalidOperationException("End must be called before Begin can be called again.");

            effect.World = world;
            effect.View = camera.View;
            effect.Projection = camera.Projection;

            // Begin the effect
            effect.CurrentTechnique.Passes[0].Apply();

            // It's now ok to call Draw and Flush
            hasBegun = true;
        }

        // Adds a point to be rendered as a quad. Must be called between Begin and End
        public void Draw(Vector3 position, float radius, Color color)
        {
            if (!hasBegun)
                throw new InvalidOperationException("Begin must be called before Draw can be called.");

            // If there's no more room to add new vertices and indices, flush the buffer
            if (vertexBufferPosition + verticesPerPoint >= vertices.Length || indexBufferPosition + indicesPerPoint >= indices.Length)
                Flush();

            vertices[vertexBufferPosition + 0].Position = new Vector3(position.X - radius, position.Y + radius, position.Z);
            vertices[vertexBufferPosition + 0].Color = color;
            vertices[vertexBufferPosition + 0].TextureCoordinate = Vector2.Zero;

            vertices[vertexBufferPosition + 1].Position = new Vector3(position.X - radius, position.Y - radius, position.Z);
            vertices[vertexBufferPosition + 1].Color = color;
            vertices[vertexBufferPosition + 1].TextureCoordinate = Vector2.UnitY;

            vertices[vertexBufferPosition + 2].Position = new Vector3(position.X + radius, position.Y + radius, position.Z);
            vertices[vertexBufferPosition + 2].Color = color;
            vertices[vertexBufferPosition + 2].TextureCoordinate = Vector2.UnitX;

            vertices[vertexBufferPosition + 3].Position = new Vector3(position.X + radius, position.Y - radius, position.Z);
            vertices[vertexBufferPosition + 3].Color = color;
            vertices[vertexBufferPosition + 3].TextureCoordinate = Vector2.One;

            indices[indexBufferPosition + 0] = (short)(vertexBufferPosition + 0);
            indices[indexBufferPosition + 1] = (short)(vertexBufferPosition + 2);
            indices[indexBufferPosition + 2] = (short)(vertexBufferPosition + 1);
            indices[indexBufferPosition + 3] = (short)(vertexBufferPosition + 2);
            indices[indexBufferPosition + 4] = (short)(vertexBufferPosition + 3);
            indices[indexBufferPosition + 5] = (short)(vertexBufferPosition + 1);

            vertexBufferPosition += verticesPerPoint;
            indexBufferPosition += indicesPerPoint;
        }

        // Finishes a batch of points to be drawn by calling Flush
        public void End()
        {
            if (!hasBegun)
                throw new InvalidOperationException("Begin must be called before End can be called.");

            // Draw the points
            Flush();

            hasBegun = false;
        }

        // Draws the buffered points
        private void Flush()
        {
            if (!hasBegun)
                throw new InvalidOperationException("Begin must be called before Flush can be called.");

            // If no points have been queued, return early
            if (vertexBufferPosition == 0 || indexBufferPosition == 0)
                return;

            // Calculate primitives to be drawn
            int primitiveCount = 2 * vertexBufferPosition / verticesPerPoint;

            // Draw the primitives
            graphicsDevice.DrawUserIndexedPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, vertices, 0, vertexBufferPosition, indices, 0, primitiveCount);

            // Reset the buffer positions
            vertexBufferPosition = 0;
            indexBufferPosition = 0;
        }
    }
}
