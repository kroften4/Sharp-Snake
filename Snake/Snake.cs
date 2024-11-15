﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    internal class Snake
    {
        public LinkedList<Vector2D> snakeTiles;
        public List<Vector2D> eatenTiles;
        public Vector2D HeadPos
        {
            get => snakeTiles.First();
        }
        public List<Vector2D> Tail
        {
            get
            {
                return snakeTiles.ToList().GetRange(1, snakeTiles.Count() - 1);
            }
        }
        public Vector2D PrevHeadPos
        {
            get => snakeTiles.ElementAt(1);
        }
        public int Length
        {
            get => snakeTiles.Count();
        }
        private const int InitialLength = 3;
        public int Score
        {
            get => Length - InitialLength;
        }
        public Direction direction;

        public Snake(LinkedList<Vector2D> snakeTiles, Direction direction, List<Vector2D> eatenTiles)
        {
            this.snakeTiles = snakeTiles;
            this.direction = direction;
            this.eatenTiles = eatenTiles;
        }

        public static Snake Create(Vector2D headPos, Direction direction)
        {
            LinkedList<Vector2D> snakeTiles = new LinkedList<Vector2D>();
            for (int x = headPos.x; headPos.x - x < InitialLength; x--)
            {
                Vector2D snakeTile = new Vector2D(x, headPos.y);
                snakeTiles.AddLast(snakeTile);
            }
            return new Snake(snakeTiles, direction, new List<Vector2D>());
        }

        private Vector2D GetNextTile(Direction direction, Vector2D boardBounds)
        {
            Vector2D deltaHeadPos;
            switch (direction)
            {
                case Direction.Left:
                    deltaHeadPos = new Vector2D(-1, 0);
                    break;
                case Direction.Right:
                    deltaHeadPos = new Vector2D(1, 0);
                    break;
                case Direction.Up:
                    deltaHeadPos = new Vector2D(0, -1);
                    break;
                case Direction.Down:
                    deltaHeadPos = new Vector2D(0, 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return HeadPos.Plus(deltaHeadPos).Modulo(boardBounds);
        }

        private Snake Move(Direction direction, Vector2D boardBounds, bool isEating = false)
        {
            Vector2D newHead = GetNextTile(direction, boardBounds);
            LinkedList<Vector2D> newSnakeTiles = new LinkedList<Vector2D>(snakeTiles);
            newSnakeTiles.AddFirst(newHead);
            if (!isEating)
            {
                eatenTiles.Remove(newSnakeTiles.Last());
                newSnakeTiles.RemoveLast();
            }
            return new Snake(newSnakeTiles, direction, eatenTiles);
        }

        public State Update(State gameState, ref List<ConsoleKey> keys)
        {
            Vector2D boardBounds = new Vector2D(gameState.level.width, gameState.level.height);
            Direction nextDirection = direction;
            for (int i = 0; i < keys.Count(); i++)
            {
                ConsoleKey key = keys[i];
                switch(key)
                {
                    case ConsoleKey.LeftArrow:
                        nextDirection = Direction.Left;
                        break;
                    case ConsoleKey.RightArrow:
                        nextDirection = Direction.Right;
                        break;
                    case ConsoleKey.UpArrow:
                        nextDirection = Direction.Up;
                        break;
                    case ConsoleKey.DownArrow:
                        nextDirection = Direction.Down;
                        break;
                }
                // Skip if a key doesn't turn the snake left or right
                if (nextDirection == direction)
                    continue;
                else if (GetNextTile(nextDirection, boardBounds) == PrevHeadPos)
                {
                    nextDirection = direction;
                    continue;
                }

                keys.RemoveRange(0, i + 1);
                break;
            }

            
            Vector2D nextTile = GetNextTile(nextDirection, boardBounds);
            bool isEating = false;
            if (gameState.food.position == nextTile)
            {
                isEating = true;
                eatenTiles.Add(nextTile);
            }
            Snake newSnake = Move(nextDirection, boardBounds, isEating);

            StateStatus newStatus = gameState.status;
            if (newSnake.Tail.Contains(newSnake.HeadPos) || gameState.level.walls.Intersect(newSnake.snakeTiles).Count() != 0)
                newStatus = StateStatus.Lost;

            return new State(newStatus, gameState.level, newSnake, gameState.food);
        }
    }
}
