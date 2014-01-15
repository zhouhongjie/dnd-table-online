﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace DndTable.Core.Test.UnitTests
{
    [TestFixture]
    public class CharacterSheetTest
    {
        [Test]
        public void ArmorClassDexTest()
        {
            var sheet = new CharacterSheet();

            // Dexterity
            sheet.Dexterity = 0;
            Assert.AreEqual(5, sheet.ArmorClass);
            sheet.Dexterity = 10;
            Assert.AreEqual(10, sheet.ArmorClass);
            sheet.Dexterity = 20;
            Assert.AreEqual(15, sheet.ArmorClass);
        }

        [Test]
        public void ArmorClassArmorTest()
        {
            var sheet = new CharacterSheet();
            sheet.Dexterity = 10;

            // Armor
            var armor = new Armor();
            sheet.EquipedArmor = armor;

            armor.ArmorBonus = 0;
            Assert.AreEqual(10, sheet.ArmorClass);

            armor.ArmorBonus = 5;
            Assert.AreEqual(15, sheet.ArmorClass);
        }
    }
}
