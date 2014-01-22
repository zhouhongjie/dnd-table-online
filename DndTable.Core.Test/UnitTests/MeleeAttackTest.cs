using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Factories;
using DndTable.Core.Test.Mocks;
using NUnit.Framework;

namespace DndTable.Core.Test.UnitTests
{
    [TestFixture]
    public class MeleeAttackTest
    {
        [Test]
        public void SimpleAttack()
        {
            var diceRoller = new MockDiceRoller();
            diceRoller.MockCheck = true; // = hit
            diceRoller.MockRoll = 5; // = damage

            var board = new Board(10, 10);
            var game = new Game(board, diceRoller);

            var char1 = Factory.CreateCharacter("dummy1");
            game.AddCharacter(char1, Position.Create(1, 1));
            game.EquipWeapon(char1, WeaponFactory.Dagger());

            var char2 = Factory.CreateCharacter("dummy2");
            game.AddCharacter(char2, Position.Create(1, 2));

            var encounter = new Encounter(board, diceRoller, new List<ICharacter>() {char1, char2});

            Assert.AreEqual(10, char2.CharacterSheet.HitPoints, "Precondition");


            var meleeAttack = new MeleeAttackAction(char1);
            meleeAttack.Initialize(diceRoller, encounter, board);


            meleeAttack.Target(char2).Do();
            Assert.AreEqual(5, char2.CharacterSheet.HitPoints);

            meleeAttack.Target(char2).Do();
            Assert.AreEqual(0, char2.CharacterSheet.HitPoints);
        }
    }
}
