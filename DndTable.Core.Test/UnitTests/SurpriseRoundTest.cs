using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Factories;
using DndTable.Core.Test.UserTests;
using DndTable.Core.Weapons;
using Moq;
using NUnit.Framework;

namespace DndTable.Core.Test.UnitTests
{
    [TestFixture]
    public class SurpriseRoundTest
    {
        [TestCase(20, 10)]
        [TestCase(10, 20)]
        public void StartEncounter(int dex1, int dex2)
        {
            var game = CreateGame(10, 10);
            var char1 = EncounterHelper.PrepareCharacter(game, "char1", Position.Create(1, 1), new Weapon() {DamageD = 4}, null);
            var char2 = EncounterHelper.PrepareCharacter(game, "char2", Position.Create(1, 2), new Weapon() {DamageD = 4}, null);

            // Adjust dexterity to influence init order
            (char1.CharacterSheet as CharacterSheet).Dexterity = dex1;
            (char2.CharacterSheet as CharacterSheet).Dexterity = dex2;

            // Char1 aware, char2 unaware
            var encounter = game.StartEncounter(
                new List<ICharacter>() {char1},
                new List<ICharacter>() {char2});

            // Surprise round: "char1" is aware = always first
            Assert.AreEqual("char1", encounter.GetCurrentCharacter().CharacterSheet.Name);
            AssertActionPossible<MoveAction>(encounter);
            AssertActionNotPossible<ChargeAction>(encounter);
        }

        [Test]
        public void NoAwareChars()
        {
            var game = CreateGame(10, 10);
            var char1 = EncounterHelper.PrepareCharacter(game, "char1", Position.Create(1, 1), new Weapon() {DamageD = 4}, null);
            var char2 = EncounterHelper.PrepareCharacter(game, "char2", Position.Create(1, 2), new Weapon() {DamageD = 4}, null);

            // Make sure char1 is first
            (char1.CharacterSheet as CharacterSheet).Dexterity = 20;

            // Char1 aware, char2 unaware
            var encounter = game.StartEncounter(
                new List<ICharacter>(),
                new List<ICharacter>() { char1, char2 });

            // No surprise round
            Assert.AreEqual("char1", encounter.GetCurrentCharacter().CharacterSheet.Name);
            AssertActionPossible<ChargeAction>(encounter);

            // Normal round
            encounter.GetNextCharacter();
            Assert.AreEqual("char2", encounter.GetCurrentCharacter().CharacterSheet.Name);
            AssertActionPossible<ChargeAction>(encounter);
        }

        [Test]
        public void TillNormalRound()
        {
            var game = CreateGame(10, 10);
            var char1 = EncounterHelper.PrepareCharacter(game, "char1", Position.Create(1, 1), new Weapon() {DamageD = 4}, null);
            var char2 = EncounterHelper.PrepareCharacter(game, "char2", Position.Create(1, 2), new Weapon() {DamageD = 4}, null);

            // Make sure char1 is first
            (char1.CharacterSheet as CharacterSheet).Dexterity = 20;

            // Char1 aware, char2 unaware
            var encounter = game.StartEncounter(
                new List<ICharacter>() { char1 },
                new List<ICharacter>() { char2 });

            // Surprise round
            Assert.AreEqual("char1", encounter.GetCurrentCharacter().CharacterSheet.Name);
            AssertActionPossible<MoveAction>(encounter);
            AssertActionNotPossible<ChargeAction>(encounter);

            // Normal round
            encounter.GetNextCharacter();
            Assert.AreEqual("char1", encounter.GetCurrentCharacter().CharacterSheet.Name);
            AssertActionPossible<ChargeAction>(encounter);

            // Normal round
            encounter.GetNextCharacter();
            Assert.AreEqual("char2", encounter.GetCurrentCharacter().CharacterSheet.Name);
            AssertActionPossible<ChargeAction>(encounter);
        }

        private static DiceRoller CreateDiceRoller()
        {
            var diceRandomizer = new Mock<IDiceRandomizer>();

            // always return 0 = no random
            diceRandomizer.Setup(dr => dr.Roll(It.IsAny<int>())).Returns(0);

            var diceRoller = new DiceRoller(diceRandomizer.Object);
            return diceRoller;
        }

        public static IGame CreateGame(int maxX, int maxY)
        {
            var board = new Board(maxX, maxY);
            return new Game(board, CreateDiceRoller());
        }

        private void AssertActionPossible<T>(IEncounter encounter) where T : BaseAction
        {
            var possibleActions = encounter.GetPossibleActionsForCurrentCharacter();
            Assert.NotNull(possibleActions.FirstOrDefault(a => a is T));
        }

        private void AssertActionNotPossible<T>(IEncounter encounter) where T : BaseAction
        {
            var possibleActions = encounter.GetPossibleActionsForCurrentCharacter();
            Assert.Null(possibleActions.FirstOrDefault(a => a is T));
        }
    }
}
