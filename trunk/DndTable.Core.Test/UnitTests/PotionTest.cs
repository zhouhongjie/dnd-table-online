using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Items;
using Moq;
using NUnit.Framework;

namespace DndTable.Core.Test.UnitTests
{
    [TestFixture]
    public class PotionTest
    {
        [Test]
        public void DrinkPotionAction()
        {
            throw new NotImplementedException("TODO"); 
        }

        [Test]
        public void PotionOfCureLightWound()
        {
            var sheet = new CharacterSheet()
                            {
                                HitPoints = 10,
                                MaxHitPoints = 20,
                            };
            var character = new Character(sheet);

            // Setup diceRoller
            var diceRandomizer = new Mock<IDiceRandomizer>();
            diceRandomizer.Setup(dr => dr.Roll(8)).Returns(5);
            var diceRoller = new DiceRoller(diceRandomizer.Object);

            // Do test
            var potion = new PotionOfCureLightWound();
            potion.Use(character, diceRoller);

            Assert.AreEqual(16, character.CharacterSheet.HitPoints, "10 + 5 (1d8) + 1 (lvl)");

            // Cannot exceed MaxHitPoints
            potion.Use(character, diceRoller);
            Assert.AreEqual(20, character.CharacterSheet.HitPoints);
            potion.Use(character, diceRoller);
            Assert.AreEqual(20, character.CharacterSheet.HitPoints);
        }
    }
}
