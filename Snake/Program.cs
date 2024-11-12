using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;

namespace Snake
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Level level = Level.BordersLevel(6, 6);
            //Vector2D snakeStartPos = new Vector2D(
            //    (int)Math.Floor(level.width * 0.4), 
            //    level.height / 2
            //);
            string levelPlan = Properties.Resources.LevelPlan;
            Vector2D snakeStartPos = new Vector2D(5, 16);
            Level level = Level.FromString(levelPlan, '#');
            State gameState = State.Create(level, snakeStartPos);
            DrawState(gameState);

            List<ConsoleKey> keys = new List<ConsoleKey>();
            const int MaxKeysListSize = 3;

            while (true)
            {
                Thread.Sleep(250);

                while (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    if (keys.Count() == 0 || keys.Last() != key)
                        keys.Add(key);
                    if (keys.Count() > MaxKeysListSize)
                        keys.RemoveAt(keys.Count() - 1);
                }

                gameState = gameState.Update(ref keys);
                DrawState(gameState);
                if (gameState.status != StateStatus.Playing)
                    break;
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(true);
        }

        static void DrawState(State gameState)
        {
            const char WallChar = '\u2588';
            const ConsoleColor WallColor = ConsoleColor.DarkBlue;
            const char FoodChar = '\u2588'; 
            const ConsoleColor FoodColor = ConsoleColor.Red;
            const char SnakeChar = '\u2588';
            const ConsoleColor SnakeColor = ConsoleColor.Green;
            const ConsoleColor SnakeHeadColor = ConsoleColor.DarkGreen;
            const ConsoleColor SnakeEatenColor = ConsoleColor.DarkYellow;

            Console.Clear();

            foreach (Vector2D wall in gameState.level.walls)
            {
                for (int i = 0; i < 2; i++)
                {
                    Console.SetCursorPosition(wall.x * 2 + i, wall.y);
                    Console.ForegroundColor = WallColor;
                    Console.WriteLine(WallChar);
                }
            }

            for (int i = 0; i < 2; i++)
            {
                Vector2D foodPos = gameState.food.position;
                Console.SetCursorPosition(foodPos.x * 2 + i, foodPos.y);
                Console.ForegroundColor = FoodColor;
                Console.WriteLine(FoodChar);
            }

            foreach (Vector2D snakePart in gameState.snake.snakeTiles)
            {
                for (int i = 0; i < 2; i++)
                {
                    Console.SetCursorPosition(snakePart.x * 2 + i, snakePart.y);
                    if (snakePart == gameState.snake.HeadPos)
                        Console.ForegroundColor = SnakeHeadColor;
                    else if (gameState.snake.eatenTiles.Contains(snakePart))
                        Console.ForegroundColor = SnakeEatenColor;
                    else
                        Console.ForegroundColor = SnakeColor;
                    Console.WriteLine(SnakeChar);

                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, gameState.level.height);
            Console.WriteLine($"Score: {gameState.snake.Score}");
            switch (gameState.status)
            {
                case StateStatus.Lost:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("GAME OVER");
                    break;
                case StateStatus.Won:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("YOU WIN");
                    break;
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
