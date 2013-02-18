using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VectorArenaWin8.Gameplay;
using VectorArenaWin8.Graphics;

namespace VectorArenaWin8
{
    class GameplayScene : Scene
    {
        public BulletManager BulletManager;

        const int worldSize = 10000;

        Ship ship;
        Starfield starfield;
        Grid grid;

        public GameplayScene(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            BulletManager = new BulletManager(100);
            ship = new Ship();
            starfield = new Starfield(worldSize);
            grid = new Grid(worldSize);
        }

        public void Initialize()
        {
            AddActor(starfield);
            AddActor(grid);
            AddActor(ship);
            AddActor(BulletManager);

            BulletManager.CreateBullets();

            Camera.Position = new Vector3(0.0f, 0.0f, 500.0f);
            Camera.TargetActor = ship;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Left))
            {
                ship.Turn(1);
            }
            if (keyboard.IsKeyDown(Keys.Right))
            {
                ship.Turn(-1);
            }
            if (keyboard.IsKeyDown(Keys.Up))
            {
                ship.Thrust(1);
            }
            else if (keyboard.IsKeyDown(Keys.Down))
            {
                ship.Thrust(-1);
            }
            else
            {
                ship.Thrust(0);
            }
            if (keyboard.IsKeyDown(Keys.Space))
            {
                ship.Fire();
            }

            base.Update(gameTime);
        }
    }
}
