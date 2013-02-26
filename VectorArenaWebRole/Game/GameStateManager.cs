using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace VectorArenaWebRole
{
    public class GameStateManager
    {
        public ConcurrentDictionary<string, object[]> GameStates(ConcurrentDictionary<string, Player> players, ConcurrentDictionary<string, Ship> ships, List<Bullet> bullets)
        {
            ConcurrentDictionary<string, object[]> gameStates = new ConcurrentDictionary<string, object[]>();

            Parallel.ForEach(players, player =>
            {
                GameState gameState = new GameState();

                Parallel.ForEach(ships, ship =>
                {
                    gameState.Ships.Add(Compress(ship.Value));
                });

                lock (bullets)
                {
                    foreach (Bullet bullet in bullets)
                    {
                        object[] compressedBullet = Compress(bullet);
                        gameState.Bullets.Add(compressedBullet);
                    }
                }

                gameStates.TryAdd(player.Key, Compress(gameState));
            });

            return gameStates;
        }

        private object[] Compress(Ship ship)
        {
            object[] compressedShip = new object[8];

            compressedShip[0] = ship.Id;
            compressedShip[1] = ship.Position.X;
            compressedShip[2] = ship.Position.Y;
            compressedShip[3] = ship.Velocity.X;
            compressedShip[4] = ship.Velocity.Y;
            compressedShip[5] = ship.Acceleration.X;
            compressedShip[6] = ship.Acceleration.Y;
            compressedShip[7] = ship.Rotation;

            return compressedShip;
        }

        private object[] Compress(Bullet bullet)
        {
            object[] compressedBullet = new object[5];

            compressedBullet[0] = bullet.Id;
            compressedBullet[1] = bullet.Position.X;
            compressedBullet[2] = bullet.Position.Y;
            compressedBullet[3] = bullet.Velocity.X;
            compressedBullet[4] = bullet.Velocity.Y;

            return compressedBullet;
        }

        private object[] Compress(GameState gameState)
        {
            object[] compressedGameState = new object[2];

            compressedGameState[0] = gameState.Ships;
            compressedGameState[1] = gameState.Bullets;

            return compressedGameState;
        }
    }
}