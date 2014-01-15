using NUnit.Framework;

namespace DndTable.Core.Test.UnitTests
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

            var char1 = Factory.CreateCharacter("dummy");
            game.AddCharacter(char1, Position.Create(1, 1));
            Assert.AreEqual(char1, game.GameBoard.GetEntity(Position.Create(1, 1)));

            game.Move(char1, Position.Create(1, 2));
            Assert.IsNull(game.GameBoard.GetEntity(Position.Create(1, 1)));
            Assert.AreEqual(char1, game.GameBoard.GetEntity(Position.Create(1, 2)));
            Assert.AreEqual(1, char1.Position.X);
            Assert.AreEqual(2, char1.Position.Y);

        }
    }
}
