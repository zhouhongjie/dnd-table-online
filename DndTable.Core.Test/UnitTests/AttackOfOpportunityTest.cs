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
    /// <summary>
    /// D&D 3.5 rules: http://www.wizards.com/default.asp?x=dnd/rg/20041102a
    /// </summary>
    [TestFixture]
    public class AttackOfOpportunityTest
    {
        [Test]
        public void RangedAttackProvokesAoO()
        {
            // Next to each other
            DoAttackWithAoO(
                Position.Create(1, 1), 
                Position.Create(1, 2), 
                CreateDiceRoller(15, 3),        // 3 damage on the D4 roll 
                3,                              // We expect a 3 damage done by AoO
                WeaponFactory.CrossbowLight(),  
                WeaponFactory.Dagger());        // damage = D4

            // Not Next to each other
            DoAttackWithAoO(
                Position.Create(1, 1), 
                Position.Create(1, 3), 
                CreateDiceRoller(15, 3),        // 3 damage on the D4 roll 
                0,                              // We expect a NO damage done by AoO
                WeaponFactory.CrossbowLight(),  
                WeaponFactory.Dagger());        // damage = D4
        }

        [Test]
        public void MeleeAttackDoesnNotProvokesAoO()
        {
            // Next to each other
            DoAttackWithAoO(
                Position.Create(1, 1), 
                Position.Create(1, 2), 
                CreateDiceRoller(15, 3),        // 3 damage on the D4 roll 
                0,                              // We expect a NO damage done by AoO
                WeaponFactory.Dagger(),         
                WeaponFactory.Dagger());        // damage = D4
        }

        [Test]
        public void NoAoO_WhenUnarmed()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void NoAoO_WithRangedWeapon()
        {
            throw new NotImplementedException();
        }

        private DiceRoller CreateDiceRoller(int d20Roll, int d4Roll)
        {
            var diceRandomizer = new Mock<IDiceRandomizer>();
            diceRandomizer.Setup(dr => dr.Roll(20)).Returns(d20Roll);
            diceRandomizer.Setup(dr => dr.Roll(4)).Returns(d4Roll); // dagger is used for AoO
            diceRandomizer.Setup(dr => dr.Roll(8)).Returns(0); // CrossbowLight does no damage

            var diceRoller = new DiceRoller(diceRandomizer.Object);
            return diceRoller;
        }


        private static void DoAttackWithAoO(
            Position attackerPosition, Position targetPosition, IDiceRoller diceRoller, int expectedOpportunityDamage, 
            IWeapon attackerWeapon, IWeapon opportunityWeapon)
        {
            var board = new Board(10, 10);
            var game = new Game(board, diceRoller);

            var char1 = Factory.CreateCharacter("dummy1");
            game.AddCharacter(char1, attackerPosition);
            game.EquipWeapon(char1, attackerWeapon); 

            var char2 = Factory.CreateCharacter("dummy2");
            game.AddCharacter(char2, targetPosition);
            game.EquipWeapon(char2, opportunityWeapon); 

            var encounter = new Encounter(board, diceRoller, new List<ICharacter>() { char1, char2 });
            var actionFactory = new AbstractActionFactory(encounter, board, diceRoller);

            Assert.AreEqual(10, char2.CharacterSheet.HitPoints, "Precondition");


            var meleeAttack = new AttackAction(char1);
            meleeAttack.Initialize(actionFactory);


            meleeAttack.Target(char2).Do();
            Assert.AreEqual(10 - expectedOpportunityDamage, char1.CharacterSheet.HitPoints);

            // Only 1 AoO allowed in 1 round
            meleeAttack.Target(char2).Do();
            Assert.AreEqual(10 - expectedOpportunityDamage, char1.CharacterSheet.HitPoints);
        }

    }
}
