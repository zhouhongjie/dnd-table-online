using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Factories;
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

        [Test]
        public void ArmorClassSizeTest()
        {
            var sheet = new CharacterSheet();
            sheet.Dexterity = 10;

            // Size
            sheet.SizeModifier = 2;
            Assert.AreEqual(12, sheet.ArmorClass);

            sheet.SizeModifier = -2;
            Assert.AreEqual(8, sheet.ArmorClass);
        }

        [Test]
        public void GetCurrentAttackBonusTest()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void GetCurrentDamageBonusTest_Melee()
        {
            var weapon = new Weapon() {IsRanged = false};

            var sheet = new CharacterSheet();
            sheet.Strength = 12;
            sheet.EquipedWeapon = weapon;
            Assert.AreEqual(1, sheet.GetCurrentDamageBonus());
            sheet.Strength = 14;
            Assert.AreEqual(2, sheet.GetCurrentDamageBonus());
        }

        [Test]
        public void GetCurrentDamageBonusTest_Range()
        {
            var weapon = new Weapon() {IsRanged = true};

            var sheet = new CharacterSheet();
            sheet.Dexterity = 12;
            sheet.EquipedWeapon = weapon;
            Assert.AreEqual(0, sheet.GetCurrentDamageBonus());
            sheet.Dexterity = 14;
            Assert.AreEqual(0, sheet.GetCurrentDamageBonus());
        }
    }
}
