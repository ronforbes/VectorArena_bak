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
        public GameStateManager GameStateManager;

        const int worldWidth = 10000;
        const int worldHeight = 10000;
        const int worldDepth = 10000;

        Starfield starfield;
        Grid grid;
        HubConnection hubConnection;
        IHubProxy hubProxy;
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
            GameStateManager = new GameStateManager();

            AddActor(ShipManager);
            AddActor(BulletManager);
            AddActor(starfield);
            AddActor(grid);

#if DEBUG
            hubConnection = new HubConnection("http://localhost:29058");
#else
            hubConnection = new HubConnection("http://vectorarena.cloudapp.net");
#endif
            hubProxy = hubConnection.CreateHubProxy("gameHub");
            hubProxy.On("Sync", data => Sync(data));
            hubConnection.Start().ContinueWith(startTask =>
            {
                hubProxy.Invoke<int>("AddPlayer").ContinueWith(invokeTask =>
                {
                    ShipManager.InitializePlayerShip(invokeTask.Result, hubProxy);
                    Camera.TargetActor = ShipManager.PlayerShip;
                    Camera.Position = new Vector3(ShipManager.PlayerShip.Position.X, ShipManager.PlayerShip.Position.Y, 500.0f);
                });
            });
        }

        void Sync(dynamic data)
        {
            GameState gameState = GameStateManager.Decompress(data);

            ShipManager.SyncShips(gameState.Ships);
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
