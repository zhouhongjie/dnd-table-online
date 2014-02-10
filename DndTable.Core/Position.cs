using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    public class Position
    {
        public static Position Create(int x, int y)
        {
            return new Position(x, y);
        }

        private Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public static bool operator ==(Position p1, Position p2)
        {
            if ((object)p1 == null)
                return (object)p2 == null;
            if ((object)p2 == null)
                return false;

            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public static bool operator !=(Position p1, Position p2)
        {
            if ((object)p1 == null)
                return (object)p2 != null;
            if ((object)p2 == null)
                return true;

            return p1.X != p2.X || p1.Y != p2.Y;
        }
    }
}
