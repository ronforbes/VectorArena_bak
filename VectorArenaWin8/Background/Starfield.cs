using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorArenaWin8
{
    class Starfield : GameObject
    {
        Random random;
        Vector3[] points;
        float[] brightnesses;

        const float starRadius = 5.0f;
        const int starCount = 100000;

        public Starfield(int width, int height, int depth)
        {
            random = new Random();

            points = new Vector3[starCount];
            brightnesses = new float[starCount];

            BoundingBox bounds = new BoundingBox(new Vector3(-1 * width / 2, -1 * height / 2, -1 * depth / 2), new Vector3(width / 2, height / 2, depth / 2));

            for (int s = 0; s < points.Length; s++)
            {
                points[s] = new Vector3(random.Next((int)bounds.Min.X, (int)bounds.Max.X),
                    random.Next((int)bounds.Min.Y, (int)bounds.Max.Y),
                    random.Next((int)bounds.Min.Z, (int)bounds.Max.Z));
                brightnesses[s] = (float)random.NextDouble();
            }
        }

        public override void Draw(Camera camera)
        {
            // Draw the starfield
            PointBatch.Begin(Matrix.Identity, camera);

            for (int s = 0; s < starCount; s++)
            {
                PointBatch.Draw(points[s], starRadius, new Color(brightnesses[s], brightnesses[s], brightnesses[s]));
            }

            PointBatch.End();
        }
    }
}
