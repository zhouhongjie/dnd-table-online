using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Factories;
using NUnit.Framework;

namespace DndTable.Core.Test.UnitTests
{
    /// <summary>
    /// D&D 3.5 rules: http://www.wizards.com/default.asp?x=dnd/rg/20041102a
    /// </summary>
    [TestFixture]
    public class MoveAttackOfOpportunityTest
    {
        [Test]
        public void LeaveOriginalArea()
        {
            var board = new Board(10, 10);
            var game = new Game(board, null);

            var char1 = Factory.CreateCharacter("dummy1", new CharacterSheet { Speed = 30 });
            var char2 = Factory.CreateCharacter("dummy2", new CharacterSheet { Speed = 30 });

            game.AddCharacter(char1, Position.Create(1, 1));
            game.AddCharacter(char2, Position.Create(1, 2));

            // Check Threatened Area

            // char1 moves => char2 does not get AoO
        }
    }
}
