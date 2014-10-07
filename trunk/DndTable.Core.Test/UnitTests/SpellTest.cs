using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Items;
using DndTable.Core.Spells;
using Moq;
using NUnit.Framework;

namespace DndTable.Core.Test.UnitTests
{
    [TestFixture]
    public class SpellTest
    {
        [Test]
        public void CastSpellAction()
        {
            throw new NotImplementedException("TODO: write test"); 
        }

        [Test]
        public void CureLightWound()
        {
            var sheet = new CharacterSheet();
            sheet.HpProperty.BaseValue = 10;
            sheet.MaxHpProperty.BaseValue = 20;
            var character = new Character(sheet);

            // Setup diceRoller
            var diceRandomizer = new Mock<IDiceRandomizer>();
            diceRandomizer.Setup(dr => dr.Roll(8)).Returns(5);
            var diceRoller = new DiceRoller(diceRandomizer.Object);

            // Do test
            var potion = new CureLightWound();
            potion.Caster = character;
            potion.CastOn(character, diceRoller);

            Assert.AreEqual(16, character.CharacterSheet.HitPoints, "10 + 5 (1d8) + 1 (lvl)");

            // Cannot exceed MaxHitPoints
            potion.CastOn(character, diceRoller);
            Assert.AreEqual(20, character.CharacterSheet.HitPoints);
            potion.CastOn(character, diceRoller);
            Assert.AreEqual(20, character.CharacterSheet.HitPoints);
        }
    }
}
