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
            DoSimpleAttack(Position.Create(1, 1), Position.Create(1, 2), CreateDiceRoller(5, true));
        }

        private MockDiceRoller CreateDiceRoller(int roll, bool hit)
        {
            var diceRoller = new MockDiceRoller();
            diceRoller.MockCheck = hit; // = hit
            diceRoller.MockRoll = roll; // = damage
            return diceRoller;
        }

        private void DoSimpleAttack(Position attackerPosition, Position targetPosition, MockDiceRoller diceRoller)
        {
            var board = new Board(10, 10);
            var game = new Game(board, diceRoller);

            var char1 = Factory.CreateCharacter("dummy1");
            game.AddCharacter(char1, attackerPosition);
            game.EquipWeapon(char1, WeaponFactory.Dagger());

            var char2 = Factory.CreateCharacter("dummy2");
            game.AddCharacter(char2, targetPosition);

            var encounter = new Encounter(board, diceRoller, new List<ICharacter>() {char1, char2});

            Assert.AreEqual(10, char2.CharacterSheet.HitPoints, "Precondition");


            var meleeAttack = new AttackAction(char1);
            meleeAttack.Initialize(diceRoller, encounter, board);


            meleeAttack.Target(char2).Do();
            Assert.AreEqual(5, char2.CharacterSheet.HitPoints);

            meleeAttack.Target(char2).Do();
            Assert.AreEqual(0, char2.CharacterSheet.HitPoints);
        }

        [TestCase(1, 1)]
        [TestCase(1, 2)]
        [TestCase(1, 3)]
        [TestCase(2, 1)]
        [TestCase(2, 3)]
        [TestCase(3, 1)]
        [TestCase(3, 2)]
        [TestCase(3, 3)]
        public void RangeTestOk(int targetPositionX, int targetPositionY)
        {
            var attackerPosition = Position.Create(2, 2);
            var targetPosition = Position.Create(targetPositionX, targetPositionY);

            DoSimpleAttack(attackerPosition, targetPosition, CreateDiceRoller(5, true));
        }

        [TestCase(0, 0)]
        [TestCase(0, 2)]
        [TestCase(0, 4)]
        [TestCase(2, 0)]
        [TestCase(4, 0)]
        [TestCase(4, 4)]
        [TestCase(4, 2)]
        [TestCase(2, 4)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OutOfRangeTest(int targetPositionX, int targetPositionY)
        {
            var attackerPosition = Position.Create(2, 2);
            var targetPosition = Position.Create(targetPositionX, targetPositionY);

            DoSimpleAttack(attackerPosition, targetPosition, CreateDiceRoller(5, true));
        }
    }
}
