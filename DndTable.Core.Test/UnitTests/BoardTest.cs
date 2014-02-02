using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Factories;
using NUnit.Framework;

namespace DndTable.Core.Test.UnitTests
{
    [TestFixture]
    public class BoardTest
    {
        [Test]
        public void VisualFoV()
        {
            var board = new Board(10, 10);

            board.AddEntity(Factory.CreateWall(), Position.Create(3, 3));
            board.AddEntity(Factory.CreateWall(), Position.Create(3, 4));

            var foV = board.CalculateFieldOfView(Position.Create(1, 1));
            DrawInConsole(foV);
        }

        private void DrawInConsole(bool[,] map)
        {
            for (var i = 0; i < map.GetLength(0); i++)
            {
                var line = "";
                for (var j = 0; j < map.GetLength(1); j++)
                {
                    line += map[i, j] ? "X" : " ";
                }
                Console.WriteLine(line);
            }
        }
    }
}
