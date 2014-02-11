using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Actions;
using NUnit.Framework;

namespace DndTable.Core.Test.UnitTests
{
    [TestFixture]
    public class MathHelperTest
    {
        [Test]
        public void Go1TileInDirection()
        {
            // x-axis
            {
                var newPos = MathHelper.Go1TileInDirection(Position.Create(1, 1), Position.Create(10, 1));
                Assert.AreEqual(2, newPos.X);
                Assert.AreEqual(1, newPos.Y);
            }
            // y-axis
            {
                var newPos = MathHelper.Go1TileInDirection(Position.Create(1, 1), Position.Create(1, 10));
                Assert.AreEqual(1, newPos.X);
                Assert.AreEqual(2, newPos.Y);
            }
            // diagonal
            {
                var newPos = MathHelper.Go1TileInDirection(Position.Create(1, 1), Position.Create(10, 10));
                Assert.AreEqual(2, newPos.X);
                Assert.AreEqual(2, newPos.Y);
            }
        }
    }
}
