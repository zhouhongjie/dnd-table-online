using NUnit.Framework;

namespace DndTable.Core.Test.UnitTests
{
    [TestFixture]
    public class GameTest
    {
        private static Game CreateGame()
        {
            var board = new Board(10, 10);
            return new Game(board, null);
        }

        [Test]
        public void AddPlayers()
        {
            var game = CreateGame();

            var char1 = Factory.CreateCharacter();
            Assert.IsTrue(game.AddCharacter(char1, Position.Create(1, 1)));
            Assert.IsFalse(game.AddCharacter(char1, Position.Create(1, 1)), "Cannot add twice");

            var char2 = Factory.CreateCharacter();
            Assert.IsFalse(game.AddCharacter(char2, Position.Create(1, 1)), "Same position");
            Assert.IsTrue(game.AddCharacter(char2, Position.Create(2, 1)));

            var char3 = Factory.CreateCharacter();
            Assert.IsFalse(game.AddCharacter(char3, Position.Create(10, 1)), "outside X");
            Assert.IsFalse(game.AddCharacter(char3, Position.Create(1, 10)), "outside Y");
        }

        [TestCase(1, 1)]
        [TestCase(1, 5)]
        [TestCase(5, 1)]
        public void GetPlayers(int x, int y)
        {
            var game = CreateGame();

            Assert.IsNull(game.GameBoard.GetEntity(Position.Create(x, y)));
            Assert.AreEqual(0, game.GetCharacters().Count);

            game.AddCharacter(Factory.CreateCharacter(), Position.Create(x, y));
            var player = game.GameBoard.GetEntity(Position.Create(x, y));
            Assert.IsNotNull(player);
            Assert.AreEqual(EntityTypeEnum.Character, player.EntityType);
            Assert.AreEqual(1, game.GetCharacters().Count);

            Assert.AreEqual(x, player.Position.X);
            Assert.AreEqual(y, player.Position.Y);
        }
    }
}
