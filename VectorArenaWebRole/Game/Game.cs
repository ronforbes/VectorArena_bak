using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;

namespace VectorArenaWebRole
{
    public class Game
    {
        public static Game Instance
        {
            get { return instance.Value; }
        }

        public static IHubContext HubContext
        {
            get { return GlobalHost.ConnectionManager.GetHubContext<GameHub>(); }
        }

        public PlayerManager PlayerManager;

        const int worldWidth = 10000;
        const int worldHeight = 10000;
        const double updatesPerSecond = 60;
        const double drawsPerSecond = 15;
        readonly static Lazy<Game> instance = new Lazy<Game>(() => new Game());
        ShipManager shipManager;
        BulletManager bulletManager;
        CollisionManager collisionManager;
        GameStateManager gameStateManager;
        Random random = new Random();
        Timer updateTimer;
        Timer drawTimer;
        DateTime previousUpdateTime = DateTime.Now;
        DateTime previousDrawTime = DateTime.Now;

        public Game()
        {
            PlayerManager = new PlayerManager();
            shipManager = new ShipManager();
            bulletManager = new BulletManager();
            collisionManager = new CollisionManager();
            gameStateManager = new GameStateManager();

            updateTimer = new Timer(1000 / updatesPerSecond);
            updateTimer.AutoReset = true;
            updateTimer.Elapsed += updateTimer_Elapsed;
            updateTimer.Start();

            drawTimer = new Timer(1000 / drawsPerSecond);
            drawTimer.AutoReset = true;
            drawTimer.Elapsed += drawTimer_Elapsed;
            drawTimer.Start();
        }

        public int AddPlayer(string connectionId)
        {
            Ship ship = new Ship(new Vector2(random.Next(worldWidth) - worldWidth / 2, random.Next(worldHeight) - worldHeight / 2), bulletManager);
            Player player = new Player(connectionId, ship);

            PlayerManager.Add(player);
            shipManager.Add(ship);
            collisionManager.Add(ship);

            return ship.Id;
        }

        void updateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TimeSpan elapsedTime = e.SignalTime - previousUpdateTime;
            previousUpdateTime = e.SignalTime;

            shipManager.Update(elapsedTime);
            bulletManager.Update(elapsedTime);
            collisionManager.Update();
        }

        void drawTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TimeSpan elapsedTime = e.SignalTime - previousDrawTime;
            previousDrawTime = e.SignalTime;

            ConcurrentDictionary<string, object[]> gameStates = gameStateManager.GameStates(PlayerManager.Players, shipManager.Ships, bulletManager.Bullets);

            foreach (string connectionId in gameStates.Keys)
            {
                PlayerManager.Player(connectionId).Sync(gameStates[connectionId], HubContext);
            }
        }
    }
}