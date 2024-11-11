using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    internal class Food
    {
        public Vector2D position;

        public Food(Vector2D position)
        {
            this.position = position;
        }

        private static List<Vector2D> GetAvailableTiles(Snake snake, Level level)
        {
            HashSet<Vector2D> occupiedTiles = new HashSet<Vector2D>();
            foreach (Vector2D tile in snake.snakeTiles)
            {
                occupiedTiles.Add(tile);
            }
            foreach (Vector2D wallPos in level.walls)
            {
                occupiedTiles.Add(wallPos);
            }
            return level.TilesGrid.Except(occupiedTiles).ToList();
        }

        public static Food Spawn(Snake snake, Level level)
        {
            List<Vector2D> freeTiles = GetAvailableTiles(snake, level);

            Random rand = new Random();
            int randIdx = rand.Next(freeTiles.Count());
            Vector2D position = freeTiles[randIdx];

            return new Food(position);
        }

        public State Update(State gameState)
        {
            if (GetAvailableTiles(gameState.snake, gameState.level).Count() == 0)
                return new State(StateStatus.Won, gameState.level, gameState.snake, gameState.food);

            Food newFood = this;
            if (gameState.snake.HeadPos == position)
                newFood = Spawn(gameState.snake, gameState.level);
            return new State(gameState.status, gameState.level, gameState.snake, newFood);
        }
    }
}
