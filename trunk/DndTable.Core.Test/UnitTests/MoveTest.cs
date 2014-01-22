using DndTable.Core.Actions;
using DndTable.Core.Factories;
using NUnit.Framework;

namespace DndTable.Core.Test.UnitTests
{
    [TestFixture]
    public class MoveTest
    {
        private static Game CreateGame(Board board)
        {
            return new Game(board, null);
        }

        [Test]
        public void SimpleMove()
        {
            var board = new Board(10, 10);
            var game = CreateGame(board);

            var char1 = Factory.CreateCharacter("dummy");
            game.AddCharacter(char1, Position.Create(1, 1));
            Assert.AreEqual(char1, game.GameBoard.GetEntity(Position.Create(1, 1)));

            var moveAction = new MoveAction(char1);
            moveAction.Initialize(null, null, board);
            moveAction.Target(Position.Create(1, 2)).Do();

            Assert.IsNull(game.GameBoard.GetEntity(Position.Create(1, 1)));
            Assert.AreEqual(char1, game.GameBoard.GetEntity(Position.Create(1, 2)));
            Assert.AreEqual(1, char1.Position.X);
            Assert.AreEqual(2, char1.Position.Y);

        }
    }
}
