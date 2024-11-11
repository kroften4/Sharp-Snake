using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    internal class Vector2D : IEquatable<Vector2D>
    {
        public int x;
        public int y;
        
        public Vector2D(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Vector2D()
        {
            x = 0;
            y = 0;
        }

        public Vector2D Plus(Vector2D other)
        {
            return new Vector2D(x + other.x, y + other.y);
        }

        public Vector2D Modulo(Vector2D other)
        {
            return new Vector2D((x % other.x + other.x) % other.x, (y % other.y + other.y) % other.y);
        }

        public override bool Equals(object obj) => this.Equals(obj as Vector2D);

        public bool Equals(Vector2D other)
        {
            if (other is null)
            {
                return false;
            }

            // Optimization for a common success case.
            if (Object.ReferenceEquals(this, other))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false.
            if (this.GetType() != other.GetType())
            {
                return false;
            }

            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defines Equals as reference equality.
            return (x == other.x) && (y == other.y);
        }

        public override int GetHashCode() => (x, y).GetHashCode();

        public static bool operator ==(Vector2D lhs, Vector2D rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Vector2D lhs, Vector2D rhs) => !(lhs == rhs);
    }
}
