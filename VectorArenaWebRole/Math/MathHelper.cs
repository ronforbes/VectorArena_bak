using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VectorArenaWebRole
{
    public class MathHelper
    {
        public static float ToRadians(float degrees)
        {
            return degrees * (float)Math.PI / 180.0f;
        }
    }
}