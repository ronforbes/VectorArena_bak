using Microsoft.AspNet.SignalR.Client.Hubs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorArenaWin8.Gameplay
{
    class ShipManager : GameObject
    {
        public Ship PlayerShip
        {
            get { return playerShip; }
        }

        Dictionary<int, Ship> ships;
        Ship playerShip;
        Dictionary<Keys, Ship.Action> keyMappings;
        KeyboardState previousKeyboardState;
        IHubProxy hubProxy;

        public ShipManager() : base()
        {
            ships = new Dictionary<int, Ship>();
            
            keyMappings = new Dictionary<Keys, Ship.Action>();
            keyMappings.Add(Keys.Left, Ship.Action.TurnLeft);
            keyMappings.Add(Keys.Right, Ship.Action.TurnRight);
            keyMappings.Add(Keys.Up, Ship.Action.ThrustForward);
            keyMappings.Add(Keys.Down, Ship.Action.ThrustBackward);
            keyMappings.Add(Keys.Space, Ship.Action.Fire);

            previousKeyboardState = Keyboard.GetState();
        }

        public void InitializePlayerShip(int id, IHubProxy hubProxy)
        {
            AddShip(id);

            playerShip = ships[id];

            this.hubProxy = hubProxy;
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
                if (!this.ships.ContainsKey(ship.Id))
                {
                    AddShip(ship.Id);
                }

                this.ships[ship.Id].Position = ship.Position;
                this.ships[ship.Id].Velocity = ship.Velocity;
                this.ships[ship.Id].Acceleration = ship.Acceleration;
                this.ships[ship.Id].Rotation = ship.Rotation;
            }
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            // Handle key presses and releases
            foreach (KeyValuePair<Keys, Ship.Action> keyMapping in keyMappings)
            {
                if (keyboardState.IsKeyDown(keyMapping.Key) && previousKeyboardState.IsKeyUp(keyMapping.Key))
                {
                    hubProxy.Invoke("StartAction", keyMapping.Value.ToString());
                    playerShip.Actions[keyMapping.Value] = true;
                }

                if (keyboardState.IsKeyUp(keyMapping.Key) && previousKeyboardState.IsKeyDown(keyMapping.Key))
                {
                    hubProxy.Invoke("StopAction", keyMapping.Value.ToString());
                    playerShip.Actions[keyMapping.Value] = false;
                }
            }

            previousKeyboardState = keyboardState;
            base.Update(gameTime);
        }
    }
}
