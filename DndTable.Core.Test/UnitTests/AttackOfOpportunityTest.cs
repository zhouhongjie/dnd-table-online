using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Factories;
using DndTable.Core.Weapons;
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
                3,                              // We expect 3 damage done by AoO
                WeaponFactory.Longbow(),  
                WeaponFactory.Dagger());        // damage = D4

            // Not Next to each other
            DoAttackWithAoO(
                Position.Create(1, 1), 
                Position.Create(1, 3), 
                CreateDiceRoller(15, 3),        // 3 damage on the D4 roll 
                0,                              // We expect NO damage done by AoO
                WeaponFactory.Longbow(),  
                WeaponFactory.Dagger());        // damage = D4

            // Same faction
            DoAttackWithAoO(
                Position.Create(1, 1),
                Position.Create(1, 2),
                CreateDiceRoller(15, 3),        // 3 damage on the D4 roll 
                0,                              // We expect NO damage done by AoO
                WeaponFactory.Longbow(),
                WeaponFactory.Dagger(),         // damage = D4
                true);
        }

        [Test]
        public void ReloadProvokesAoO()
        {
            // Unload x-bow
            var xBow = WeaponFactory.CrossbowLight();
            (xBow as Weapon).ReloadInfo.IsLoaded = false;

            // Next to each other
            DoReloadWithAoO(
                Position.Create(1, 1),
                Position.Create(1, 2),
                CreateDiceRoller(15, 3), // 3 damage on the D4 roll 
                3, // We expect 3 damage done by AoO
                xBow,
                WeaponFactory.Dagger()); // damage = D4

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
            // Next to each other
            DoAttackWithAoO(
                Position.Create(1, 1),
                Position.Create(1, 2),
                CreateDiceRoller(15, 3),        // 3 damage on the D4 roll 
                0,                              // We expect a NO damage done by AoO
                WeaponFactory.Longbow(),
                null);                          // damage = ??
        }

        [Test]
        public void NoAoO_WithRangedWeapon()
        {
            // Next to each other
            DoAttackWithAoO(
                Position.Create(1, 1),
                Position.Create(1, 2),
                CreateDiceRoller(15, 3),                        // 3 damage on the D4 roll 
                0,                                              // We expect a NO damage done by AoO
                WeaponFactory.Longbow(),
                new Weapon() { IsRanged = true, DamageD = 4 });   // damage = 4
        }

        [Test]
        public void NoAoA_WhenIncapacitated()
        {
            var setup = PrepareBoard(
                CreateDiceRoller(15, 3),
                Position.Create(1, 1), 
                WeaponFactory.Longbow(),
                Position.Create(1, 2),
                WeaponFactory.Dagger(), 
                false);

            // Incapacitate AoO executer
            (setup.AoOExecuter.CharacterSheet as CharacterSheet).HitPoints = 0;

            var attack = new AttackAction(setup.ActionExecuter);
            attack.Initialize(setup.ActionFactory);

            attack.Target(setup.AoOExecuter).Do();
            Assert.AreEqual(10, setup.ActionExecuter.CharacterSheet.HitPoints, "AoO should not have been done: executer can't act");
        }

        [Test]
        public void DrinkPotionProvokesAoO()
        {
            throw new NotImplementedException("TODO");
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
            IWeapon attackerWeapon, IWeapon opportunityWeapon, bool sameFaction = false)
        {
            var setup = PrepareBoard(diceRoller, attackerPosition, attackerWeapon, targetPosition, opportunityWeapon, sameFaction);

            Assert.AreEqual(10, setup.ActionExecuter.CharacterSheet.HitPoints, "Precondition");


            var attack = new AttackAction(setup.ActionExecuter);
            attack.Initialize(setup.ActionFactory);


            attack.Target(setup.AoOExecuter).Do();
            Assert.AreEqual(10 - expectedOpportunityDamage, setup.ActionExecuter.CharacterSheet.HitPoints, "AoO failed");

            // Only 1 AoO allowed in 1 round
            attack.Target(setup.AoOExecuter).Do();
            Assert.AreEqual(10 - expectedOpportunityDamage, setup.ActionExecuter.CharacterSheet.HitPoints, "double AoO");
        }

        private static void DoReloadWithAoO(
            Position attackerPosition, Position targetPosition, IDiceRoller diceRoller, int expectedOpportunityDamage, 
            IWeapon attackerWeapon, IWeapon opportunityWeapon, bool sameFaction = false)
        {
            var setup = PrepareBoard(diceRoller, attackerPosition, attackerWeapon, targetPosition, opportunityWeapon, sameFaction);

            Assert.AreEqual(10, setup.ActionExecuter.CharacterSheet.HitPoints, "Precondition");


            var reload = new ReloadAction(setup.ActionExecuter);
            reload.Initialize(setup.ActionFactory);

            reload.Do();
            Assert.AreEqual(10 - expectedOpportunityDamage, setup.ActionExecuter.CharacterSheet.HitPoints, "AoO failed");
        }

        class PreparedSetup
        {
            public ICharacter ActionExecuter;
            public ICharacter AoOExecuter;
            public AbstractActionFactory ActionFactory;
        }

        private static PreparedSetup PrepareBoard(IDiceRoller diceRoller, Position attackerPosition, IWeapon attackerWeapon, Position targetPosition, IWeapon opportunityWeapon, bool sameFaction)
        {
            var board = new Board(10, 10);
            var game = new Game(board, diceRoller);

            var char1 = Factory.CreateCharacter("dummy1");
            game.AddCharacter(char1, attackerPosition);
            char1.EquipWeapon(attackerWeapon);

            var char2 = Factory.CreateCharacter("dummy2");
            game.AddCharacter(char2, targetPosition);
            char2.EquipWeapon(opportunityWeapon);

            // Dirty downcasting
            if (!sameFaction)
                (char2.CharacterSheet as CharacterSheet).FactionId += 1;

            var encounter = new Encounter(board, diceRoller, new List<ICharacter>() { char1, char2 }, new List<ICharacter>());
            var actionFactory = new AbstractActionFactory(encounter, board, diceRoller);

            return new PreparedSetup { ActionExecuter = char1, AoOExecuter = char2, ActionFactory = actionFactory };
        }
    }
}
