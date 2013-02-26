using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorArenaWin8
{
    class GameState
    {
        public List<Ship> Ships;
        public List<Bullet> Bullets;

        public GameState()
        {
            Ships = new List<Ship>();
            Bullets = new List<Bullet>();
        }
    }
}
