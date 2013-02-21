using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace VectorArenaServer
{
    public class GameStateManager
    {
        public ConcurrentDictionary<string, object[]> GameStates(ConcurrentDictionary<string, Player> players, ConcurrentDictionary<string, Ship> ships)
        {
            ConcurrentDictionary<string, object[]> gameStates = new ConcurrentDictionary<string, object[]>();

            Parallel.ForEach(players, player =>
            {
                GameState gameState = new GameState();

                Parallel.ForEach(ships, ship =>
                {
                    gameState.Ships.Add(Compress(ship.Value));
                });

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

        private object[] Compress(GameState gameState)
        {
            object[] compressedGameState = new object[1];

            compressedGameState[0] = gameState.Ships;

            return compressedGameState;
        }
    }
}