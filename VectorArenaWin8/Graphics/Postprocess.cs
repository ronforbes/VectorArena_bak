using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorArenaWin8.Graphics
{
    abstract class Postprocess
    {
        public abstract void Begin(RenderTarget2D scene);
        public abstract void End(RenderTarget2D scene);
    }
}
