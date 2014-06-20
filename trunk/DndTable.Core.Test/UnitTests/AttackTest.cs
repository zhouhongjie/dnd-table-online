using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Factories;
using Moq;
using NUnit.Framework;

namespace DndTable.Core.Test.UnitTests
{
    [TestFixture]
    public class AttackTest
    {
        [Test]
        public void SimpleAttack()
        {
            DoSimpleAttack(Position.Create(1, 1), Position.Create(1, 2), CreateDiceRoller(15, 4), 4);
        }

        [Test]
        public void CriticalAttack()
        {
            DoSimpleAttack(Position.Create(1, 1), Position.Create(1, 2), CreateDiceRoller(20, 4), 8);
        }

        private DiceRoller CreateDiceRoller(int d20Roll, int d4Roll)
        {
            var diceRandomizer = new Mock<IDiceRandomizer>();
            diceRandomizer.Setup(dr => dr.Roll(20)).Returns(d20Roll);
            diceRandomizer.Setup(dr => dr.Roll(4)).Returns(d4Roll);

            var diceRoller = new DiceRoller(diceRandomizer.Object);
            return diceRoller;
        }

        private void DoSimpleAttack(Position attackerPosition, Position targetPosition, IDiceRoller diceRoller, int expectedDamage)
        {
            var board = new Board(10, 10);
            var game = new Game(board, diceRoller);

            var char1 = Factory.CreateCharacter("dummy1");
            game.AddCharacter(char1, attackerPosition);
            game.EquipWeapon(char1, WeaponFactory.Dagger()); // damage = D4

            var char2 = Factory.CreateCharacter("dummy2");
            game.AddCharacter(char2, targetPosition);

            var encounter = new Encounter(board, diceRoller, new List<ICharacter>() {char1, char2});
            var actionFactory = new AbstractActionFactory(encounter, board, diceRoller);

            Assert.AreEqual(10, char2.CharacterSheet.HitPoints, "Precondition");


            var meleeAttack = new AttackAction(char1);
            meleeAttack.Initialize(actionFactory);


            meleeAttack.Target(char2).Do();
            Assert.AreEqual(10 - expectedDamage, char2.CharacterSheet.HitPoints);

            meleeAttack.Target(char2).Do();
            Assert.AreEqual(10 - 2 * expectedDamage, char2.CharacterSheet.HitPoints);
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

            DoSimpleAttack(attackerPosition, targetPosition, CreateDiceRoller(15, 4), 4);
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

            DoSimpleAttack(attackerPosition, targetPosition, CreateDiceRoller(15, 4), 4);
        }

        [Test]
        public void RangeAttackTest()
        {
            var diceRandomizer = new Mock<IDiceRandomizer>();
            diceRandomizer.Setup(dr => dr.Roll(20)).Returns(15); // Hit!
            diceRandomizer.Setup(dr => dr.Roll(8)).Returns(5); // Damage = 5

            var diceRoller = new DiceRoller(diceRandomizer.Object);
            var board = new Board(10, 10);
            var game = new Game(board, diceRoller);

            var char1 = Factory.CreateCharacter("dummy1");
            game.AddCharacter(char1, Position.Create(1, 1));
            game.EquipWeapon(char1, WeaponFactory.CrossbowLight());

            var char2 = Factory.CreateCharacter("dummy2");
            game.AddCharacter(char2, Position.Create(5, 5));

            var encounter = new Encounter(board, diceRoller, new List<ICharacter>() { char1, char2 });
            var actionFactory = new AbstractActionFactory(encounter, board, diceRoller);


            // Preconditions
            Assert.AreEqual(10, char2.CharacterSheet.HitPoints);
            Assert.IsFalse(char1.CharacterSheet.EquipedWeapon.NeedsReload);


            var rangeAttack = new AttackAction(char1);
            rangeAttack.Initialize(actionFactory);

            rangeAttack.Target(char2).Do();

            // Test post conditions
            Assert.AreEqual(5, char2.CharacterSheet.HitPoints, "Is hit");
            Assert.IsTrue(char1.CharacterSheet.EquipedWeapon.NeedsReload, "needs reload");
        }
    }
}
