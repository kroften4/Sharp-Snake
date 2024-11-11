using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    internal class Level
    {
        public readonly int width;
        public readonly int height;
        public HashSet<Vector2D> walls;
        public List<Vector2D> TilesGrid
        {
            get
            {
                List<Vector2D> value = new List<Vector2D>();
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        value.Add(new Vector2D(x, y));
                    }
                }
                return value;
            }
        }

        public Level(int width, int height, HashSet<Vector2D> walls)
        {
            this.width = width;
            this.height = height;
            this.walls = walls;
        }

        public static Level BordersLevel(int width, int height)
        {
            HashSet<Vector2D> walls = new HashSet<Vector2D>();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                        walls.Add(new Vector2D(x, y));
                }
            }
            return new Level(width, height, walls);
        }

        public static Level FromString(string levelPlan, char wallChar)
        {
            List<string> lines = levelPlan.Split('\n').ToList();
            int width = lines[0].Length;
            int height = lines.Count();
            HashSet<Vector2D> walls = new HashSet<Vector2D>();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (lines[y][x] == wallChar)
                        walls.Add(new Vector2D(x, y));
                }
            }
            return new Level(width, height, walls);
        }
    }
}
