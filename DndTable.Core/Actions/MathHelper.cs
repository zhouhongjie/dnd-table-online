﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core.Actions
{
    internal static class MathHelper
    {
        static public Position Go1TileInDirection(Position startPosition, Position endPosition)
        {
            var angle = Math.Atan2(endPosition.Y - startPosition.Y, endPosition.X - startPosition.X);// *Rad2Deg;

            var newX = startPosition.X + Math.Round(Math.Cos(angle));
            var newY = startPosition.Y + Math.Round(Math.Sin(angle));

            return Position.Create((int)newX, (int)newY);
        }

        public static double GetDistance(Position position1, Position position2)
        {
            var dx = position1.X - position2.X;
            var dy = position1.Y - position2.Y;

            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static int GetTilesDistance(Position position1, Position position2)
        {
            return (int)Math.Floor(GetDistance(position1, position2));
        }


        //static private double LinearInterpolation(double x, double x0, double x1, double y0, double y1)
        //{
        //    if ((x1 - x0) == 0)
        //    {
        //        return (y0 + y1) / 2;
        //    }
        //    return y0 + (x - x0) * (y1 - y0) / (x1 - x0);
        //}
    }
}
