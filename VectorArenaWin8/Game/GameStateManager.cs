using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorArenaWin8
{
    class GameStateManager
    {
        public GameState Decompress(dynamic gameState)
        {
            GameState decompressedGameState = new GameState();
            JContainer container = gameState;

            // Decompress ships
            JArray ships = (JArray)container[0];
            
            foreach(JToken token in ships)
            {
                Ship ship = DecompressShip(token);

                if (ship != null)
                {
                    decompressedGameState.Ships.Add(ship);
                }
            }

            // Decompress bullets
            if (container.Count() > 1)
            {
                JArray bullets = (JArray)container.ElementAt(1);

                foreach (JToken token in bullets)
                {
                    Bullet bullet = DecompressBullet(token);

                    if (bullet != null)
                    {
                        decompressedGameState.Bullets.Add(bullet);
                    }
                }
            }
            
            return decompressedGameState;
        }

        public Ship DecompressShip(JToken token)
        {
            Ship ship = null;

            if (token.Count<object>() != 0)
            {
                ship = new Ship((int)token[0]);
                ship.Position.X = (float)token[1];
                ship.Position.Y = (float)token[2];
                ship.Velocity.X = (float)token[3];
                ship.Velocity.Y = (float)token[4];
                ship.Acceleration.X = (float)token[5];
                ship.Acceleration.Y = (float)token[6];
                ship.Rotation = (float)token[7];
                ship.Alive = (bool)token[8];
                ship.Health = (float)token[9];
            }
                
            return ship;
        }

        public Bullet DecompressBullet(JToken token)
        {
            Bullet bullet = null;

            if(token.Count<object>() != 0)
            {
                bullet = new Bullet((int)token[0]);
                bullet.Position.X = (float)token[1];
                bullet.Position.Y = (float)token[2];
                bullet.Velocity.X = (float)token[3];
                bullet.Velocity.Y = (float)token[4];
            }

            return bullet;
        }
    }
}
