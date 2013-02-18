using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorArenaWin8.Gameplay
{
    class Grid : Actor
    {
        const int primaryLineInterval = 200;
        const int primaryLineWidth = 10;
        Color primaryLineColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
        const int secondaryLineInterval = 100;
        const int secondaryLineWidth = 5;
        Color secondaryLineColor = new Color(0.1f, 0.1f, 0.1f, 0.5f);
        const int tertiaryLineInterval = 50;
        const int tertiaryLineWidth = 3;
        Color tertiaryLineColor = new Color(0.05f, 0.05f, 0.05f, 0.5f);

        List<Vector3> startPoints;
        List<Vector3> endPoints;
        List<int> widths;
        List<Color> colors;
        BoundingBox bounds;

        public Grid(int size)
            : base()
        {
            startPoints = new List<Vector3>();
            endPoints = new List<Vector3>();
            widths = new List<int>();
            colors = new List<Color>();
            bounds = new BoundingBox(new Vector3(-1 * size, -1 * size, 0.0f), new Vector3(size, size, 0.0f));

            for (int x = (int)bounds.Min.X; x <= bounds.Max.X; x += tertiaryLineInterval)
            {
                startPoints.Add(new Vector3(x, bounds.Min.Y, bounds.Min.Z));
                endPoints.Add(new Vector3(x, bounds.Max.Y, bounds.Max.Z));

                if (Math.Abs(x) % primaryLineInterval == 0)
                {
                    widths.Add(primaryLineWidth);
                    colors.Add(primaryLineColor);
                }
                else if (Math.Abs(x) % primaryLineInterval == secondaryLineInterval)
                {
                    widths.Add(secondaryLineWidth);
                    colors.Add(secondaryLineColor);
                }
                else if (Math.Abs(x) % primaryLineInterval == tertiaryLineInterval)
                {
                    widths.Add(tertiaryLineWidth);
                    colors.Add(tertiaryLineColor);
                }
                else if (Math.Abs(x) % primaryLineInterval == secondaryLineInterval + tertiaryLineInterval)
                {
                    widths.Add(tertiaryLineWidth);
                    colors.Add(tertiaryLineColor);
                }
            }

            for (int y = (int)bounds.Min.Y; y <= bounds.Max.Y; y += tertiaryLineInterval)
            {
                startPoints.Add(new Vector3(bounds.Min.X, y, bounds.Min.Z));
                endPoints.Add(new Vector3(bounds.Max.X, y, bounds.Max.Z));

                if (Math.Abs(y) % primaryLineInterval == 0)
                {
                    widths.Add(primaryLineWidth);
                    colors.Add(primaryLineColor);
                }
                else if (Math.Abs(y) % primaryLineInterval == secondaryLineInterval)
                {
                    widths.Add(secondaryLineWidth);
                    colors.Add(secondaryLineColor);
                }
                else if (Math.Abs(y) % primaryLineInterval == tertiaryLineInterval)
                {
                    widths.Add(tertiaryLineWidth);
                    colors.Add(tertiaryLineColor);
                }
                else if (Math.Abs(y) % primaryLineInterval == secondaryLineInterval + tertiaryLineInterval)
                {
                    widths.Add(tertiaryLineWidth);
                    colors.Add(tertiaryLineColor);
                }
            }
        }

        public override void Draw(Camera camera)
        {
            LineBatch.Begin(Matrix.Identity, camera);

            for (int i = 0; i < startPoints.Count; i++)
            {
                LineBatch.Draw(startPoints[i], endPoints[i], widths[i], colors[i]);
            }

            LineBatch.Draw(new Vector3(bounds.Min.X, bounds.Min.Y, bounds.Min.Z), new Vector3(bounds.Max.X, bounds.Min.Y, bounds.Max.Z), 5.0f, Color.White);
            LineBatch.Draw(new Vector3(bounds.Min.X, bounds.Max.Y, bounds.Min.Z), new Vector3(bounds.Max.X, bounds.Max.Y, bounds.Max.Z), 5.0f, Color.White);
            LineBatch.Draw(new Vector3(bounds.Min.X, bounds.Min.Y, bounds.Min.Z), new Vector3(bounds.Min.X, bounds.Max.Y, bounds.Max.Z), 5.0f, Color.White);
            LineBatch.Draw(new Vector3(bounds.Max.X, bounds.Min.Y, bounds.Min.Z), new Vector3(bounds.Max.X, bounds.Max.Y, bounds.Max.Z), 5.0f, Color.White);

            LineBatch.End();
        }
    }
}
