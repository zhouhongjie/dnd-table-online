using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Factories;
using DndTable.Core.Test.Helpers;
using DndTable.Core.Test.UserTests;
using DndTable.Core.Weapons;
using NUnit.Framework;

namespace DndTable.Core.Test.UnitTests
{
    [TestFixture]
    public class FlatFootedTest
    {
        [Test]
        public void RegularRoundProgression()
        {
            var game = Factory.CreateGame(10, 10);
            var char1 = EncounterHelper.PrepareCharacter(game, "char1", Position.Create(1, 1), new Weapon() { DamageD = 4 }, null);
            var char2 = EncounterHelper.PrepareCharacter(game, "char2", Position.Create(1, 2), new Weapon() { DamageD = 4 }, null);

            var allCharacters = new List<ICharacter>() { char1, char2 };
            var encounter = game.StartEncounter(allCharacters);

            // Start
            {
                var current = encounter.GetCurrentCharacter();
                Assert.IsFalse(current.CharacterSheet.Conditions.IsFlatFooted);
                var other = EncounterHelper.GetOtherCharacter(current, allCharacters);
                Assert.IsTrue(other.CharacterSheet.Conditions.IsFlatFooted);
            }

            // next round
            {
                var current = encounter.GetNextCharacter();
                Assert.IsFalse(current.CharacterSheet.Conditions.IsFlatFooted);
                var other = EncounterHelper.GetOtherCharacter(current, allCharacters);
                Assert.IsFalse(other.CharacterSheet.Conditions.IsFlatFooted);
            }
        }

        [Test]
        public void SurpriseRoundProgression()
        {
            var game = Factory.CreateGame(10, 10);
            var char1 = EncounterHelper.PrepareCharacter(game, "char1", Position.Create(1, 1), new Weapon() { DamageD = 4 }, null);
            var char2 = EncounterHelper.PrepareCharacter(game, "char2", Position.Create(1, 2), new Weapon() { DamageD = 4 }, null);

            // Char1 will be first
            CharacterSheet.GetEditableSheet(char1).Dexterity = 100;

            var allCharacters = new List<ICharacter>() { char1, char2 };
            var encounter = game.StartEncounter(
                new List<ICharacter>() { char1 }, 
                new List<ICharacter>() { char2 });

            // Surprise round
            {
                var current = encounter.GetCurrentCharacter();
                Assert.IsFalse(current.CharacterSheet.Conditions.IsFlatFooted);
                var other = EncounterHelper.GetOtherCharacter(current, allCharacters);
                Assert.IsTrue(other.CharacterSheet.Conditions.IsFlatFooted);
            }

            // First regular
            {
                var current = encounter.GetNextCharacter();
                Assert.IsFalse(current.CharacterSheet.Conditions.IsFlatFooted);
                var other = EncounterHelper.GetOtherCharacter(current, allCharacters);
                Assert.IsTrue(other.CharacterSheet.Conditions.IsFlatFooted);
            }

            // Second regular
            {
                var current = encounter.GetNextCharacter();
                Assert.IsFalse(current.CharacterSheet.Conditions.IsFlatFooted);
                var other = EncounterHelper.GetOtherCharacter(current, allCharacters);
                Assert.IsFalse(other.CharacterSheet.Conditions.IsFlatFooted);
            }
        }

        [Test]
        public void CharacterSheetInfluence()
        {
            var game = Factory.CreateGame(10, 10);
            var char1 = EncounterHelper.PrepareCharacter(game, "char1", Position.Create(1, 1), new Weapon() {DamageD = 4}, null);

            var sheet = CharacterSheet.GetEditableSheet(char1);
            sheet.Dexterity = 20;

            sheet.EditableConditions.IsFlatFooted = false;
            Assert.AreEqual(15, sheet.GetCurrentArmorClass());
            Assert.IsFalse(sheet.LooseDexBonusToAC());

            sheet.EditableConditions.IsFlatFooted = true;
            Assert.AreEqual(10, sheet.GetCurrentArmorClass());
            Assert.IsTrue(sheet.LooseDexBonusToAC());
        }

    }
}
