using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VectorArenaServer
{
    public class Vector2
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2()
        {
            X = 0;
            Y = 0;
        }

        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 Zero
        {
            get { return new Vector2(); }
        }

        public double Length()
        {
            return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
        }

        public Vector2 Normalize()
        {
            double length = Length();
            return new Vector2(X / length, Y / length);
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2 operator *(Vector2 vector, double scalar)
        {
            return new Vector2(vector.X * scalar, vector.Y * scalar);
        }
    }
}