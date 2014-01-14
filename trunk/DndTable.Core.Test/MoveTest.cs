using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace DndTable.Core.Test
{
    [TestFixture]
    public class MoveTest
    {
        private static Game CreateGame()
        {
            var board = new Board(10, 10);
            return new Game(board, null);
        }

        [Test]
        public void SimpleMove()
        {
            var game = CreateGame();

            var char1 = Factory.CreateCharacter();
            game.AddCharacter(char1, new Position(1, 1));
            Assert.AreEqual(char1, game.GameBoard.GetEntity(new Position(1, 1)));

            game.Move(char1, new Position(1, 2));
            Assert.IsNull(game.GameBoard.GetEntity(new Position(1, 1)));
            Assert.AreEqual(char1, game.GameBoard.GetEntity(new Position(1, 2)));
            Assert.AreEqual(1, char1.Position.X);
            Assert.AreEqual(2, char1.Position.Y);

        }
    }
}
