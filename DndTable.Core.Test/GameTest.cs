using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace DndTable.Core.Test
{
    [TestFixture]
    public class GameTest
    {
        [Test]
        public void AddPlayers()
        {
            var game = Factory.CreateGame(10, 10);

            var char1 = Factory.CreateCharacter();
            Assert.IsTrue(game.AddCharacter(char1, 1, 1));
            Assert.IsFalse(game.AddCharacter(char1, 1, 1), "Cannot add twice");

            var char2 = Factory.CreateCharacter();
            Assert.IsFalse(game.AddCharacter(char2, 1, 1), "Same position");
            Assert.IsTrue(game.AddCharacter(char2, 2, 1));

            var char3 = Factory.CreateCharacter();
            Assert.IsFalse(game.AddCharacter(char3, 10, 1), "outside X");
            Assert.IsFalse(game.AddCharacter(char3, 1, 10), "outside Y");
        }

        [Test]
        public void GetPlayers()
        {
            var game = Factory.CreateGame(10, 10);

            Assert.IsNull(game.GameBoard.GetEntity(1, 1));

            game.AddCharacter(Factory.CreateCharacter(), 1, 1);
            var player = game.GameBoard.GetEntity(1, 1);
            Assert.IsNotNull(player);
            Assert.AreEqual(EntityTypeEnum.Character, player.EntityType);
        }
    }
}
