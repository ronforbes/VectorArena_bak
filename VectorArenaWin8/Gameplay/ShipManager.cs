using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorArenaWin8.Gameplay
{
    class ShipManager : Actor
    {
        public Ship PlayerShip
        {
            get { return playerShip; }
        }

        Dictionary<int, Ship> ships;
        Ship playerShip;

        public ShipManager() : base()
        {
            ships = new Dictionary<int, Ship>();
        }

        public void InitializePlayerShip(int id)
        {
            AddShip(id);

            playerShip = ships[id];
        }

        public void AddShip(int id)
        {
            if (!ships.ContainsKey(id))
            {
                ships.Add(id, new Ship(id));
                AddChild(ships[id]);
            }
        }

        public void RemoveShip(int id)
        {
            if (ships.ContainsKey(id))
            {
                ships.Remove(id);
            }
        }

        public void SyncShips(List<Ship> ships)
        {
            foreach (Ship ship in ships)
            {
                this.ships[ship.Id].Position = ship.Position;
                this.ships[ship.Id].Velocity = ship.Velocity;
                this.ships[ship.Id].Acceleration = ship.Acceleration;
                this.ships[ship.Id].Rotation = ship.Rotation;
            }
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Left))
            {
                playerShip.Turn(1);
            }
            if (keyboard.IsKeyDown(Keys.Right))
            {
                playerShip.Turn(-1);
            }
            if (keyboard.IsKeyDown(Keys.Up))
            {
                playerShip.Thrust(1);
            }
            else if (keyboard.IsKeyDown(Keys.Down))
            {
                playerShip.Thrust(-1);
            }
            else
            {
                if (playerShip != null)
                {
                    playerShip.Thrust(0);
                }
            }
            if (keyboard.IsKeyDown(Keys.Space))
            {
                if (playerShip != null)
                {
                    playerShip.Fire();
                }
            }

            base.Update(gameTime);
        }
    }
}
