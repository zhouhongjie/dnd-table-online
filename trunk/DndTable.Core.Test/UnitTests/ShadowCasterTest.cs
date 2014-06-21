using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace DndTable.Core.Test.UnitTests
{
    [TestFixture]
    public class ShadowCasterTest
    {
        [Test]
        public void VisualTest()
        {
            int maxX = 20;
            int maxY = 20;

            bool[,] map = CreateMap(maxX, maxY);


            var playerPosition = Position.Create(1, 1);

            map[5, 5] = true;
            map[5, 7] = true;
            map[5, 9] = true;
 
            bool[,] lit = new bool[maxX, maxY];

            int radius = 100; // infinite
            ShadowCaster.ComputeFieldOfViewWithShadowCasting(
                playerPosition.X, playerPosition.Y, radius,
                (x1, y1) => map[x1, y1],
                (x2, y2) => { lit[x2, y2] = true; });


            DrawInConsole(map);
            Console.WriteLine();
            DrawInConsole(lit);
        }

        private void DrawInConsole(bool[,] map)
        {
            for (var i=0; i < map.GetLength(0); i++)
            {
                var line = "";
                for (var j=0; j < map.GetLength(1); j++)
                {
                    line += map[i, j] ? "X" : " ";
                }
                Console.WriteLine(line);
            }
        }

        private bool[,] CreateMap(int maxX, int maxY)
        {
            bool[,] map = new bool[maxX, maxY];

            // Create outerwalls
            for (var i = 0; i < maxX; i++)
            {
                map[i, 0] = true;
                map[i, maxY-1] = true;
            }
            for (var i = 0; i < maxY; i++)
            {
                map[0, i] = true;
                map[maxX - 1, i] = true;
            }
            return map;
        }
    }
}
