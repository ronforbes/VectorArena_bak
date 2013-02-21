using Microsoft.AspNet.SignalR.Client.Hubs;
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
        public ShipManager ShipManager;
        public BulletManager BulletManager;

        const int worldWidth = 10000;
        const int worldHeight = 10000;
        const int worldDepth = 10000;

        Starfield starfield;
        Grid grid;
        HubConnection connection;
        IHubProxy proxy;
        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        public GameplayScene(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            starfield = new Starfield(worldWidth, worldHeight, worldDepth);
            grid = new Grid(worldWidth, worldHeight);
            ShipManager = new ShipManager();
            BulletManager = new BulletManager();

            AddActor(ShipManager);
            AddActor(BulletManager);
            AddActor(starfield);
            AddActor(grid);

            connection = new HubConnection("http://localhost:2697");
            proxy = connection.CreateHubProxy("gameHub");
            proxy.On("sync", data => Sync(data));
            connection.Start().ContinueWith(startTask =>
            {
                proxy.Invoke<int>("AddPlayer").ContinueWith(invokeTask =>
                {
                    ShipManager.InitializePlayerShip(invokeTask.Result);
                    Camera.TargetActor = ShipManager.PlayerShip;
                    Camera.Position = new Vector3(ShipManager.PlayerShip.Position.X, ShipManager.PlayerShip.Position.Y, 500.0f);
                });
            });
        }

        void Sync(dynamic data)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }

            base.Update(gameTime);
        }

        public override void Draw()
        {
            frameCounter++;

            base.Draw();
        }
    }
}
