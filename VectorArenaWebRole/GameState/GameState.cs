using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VectorArenaWebRole
{
    public class GameState
    {
        public List<object> Ships;
        public List<object> Bullets;

        public GameState()
        {
            Ships = new List<object>();
            Bullets = new List<object>(1);
        }
    }
}