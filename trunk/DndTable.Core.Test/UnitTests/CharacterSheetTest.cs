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
            Assert.AreEqual(5, sheet.GetCurrentArmorClass());
            sheet.Dexterity = 10;
            Assert.AreEqual(10, sheet.GetCurrentArmorClass());
            sheet.Dexterity = 20;
            Assert.AreEqual(15, sheet.GetCurrentArmorClass());
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
            Assert.AreEqual(10, sheet.GetCurrentArmorClass());

            armor.ArmorBonus = 5;
            Assert.AreEqual(15, sheet.GetCurrentArmorClass());
        }

        [Test]
        public void ArmorClassSizeTest()
        {
            var sheet = new CharacterSheet();
            sheet.Dexterity = 10;

            // Size
            sheet.SizeModifier = 2;
            Assert.AreEqual(12, sheet.GetCurrentArmorClass());

            sheet.SizeModifier = -2;
            Assert.AreEqual(8, sheet.GetCurrentArmorClass());
        }

        [Test]
        public void GetCurrentAttackBonusTest_Melee()
        {
            var weapon = new Weapon() { IsRanged = false };

            var sheet = new CharacterSheet();
            sheet.Strength = 12;
            sheet.Dexterity = 20;
            sheet.EquipedWeapon = weapon;
            Assert.AreEqual(1, sheet.GetCurrentAttackBonus(5));
            sheet.Strength = 14;
            sheet.Dexterity = 20;
            Assert.AreEqual(2, sheet.GetCurrentAttackBonus(5));
        }

        [Test]
        public void GetCurrentAttackBonusTest_Ranged()
        {
            var weapon = new Weapon() {IsRanged = true, RangeIncrement = 100};

            // Test Dex
            var sheet = new CharacterSheet();
            sheet.Strength = 20;
            sheet.Dexterity = 12;
            sheet.EquipedWeapon = weapon;
            Assert.AreEqual(1, sheet.GetCurrentAttackBonus(10));
            sheet.Strength = 20;
            sheet.Dexterity = 14;
            Assert.AreEqual(2, sheet.GetCurrentAttackBonus(10));

            // Test RangeIncrement
            Assert.AreEqual(2, sheet.GetCurrentAttackBonus(99));
            Assert.AreEqual(0, sheet.GetCurrentAttackBonus(100));
            Assert.AreEqual(0, sheet.GetCurrentAttackBonus(199));
            Assert.AreEqual(-2, sheet.GetCurrentAttackBonus(200));
            Assert.AreEqual(-2, sheet.GetCurrentAttackBonus(299));
            Assert.AreEqual(-4, sheet.GetCurrentAttackBonus(300));
        }

        [Test]
        public void GetCurrentDamageBonusTest_Melee()
        {
            var weapon = new Weapon() {IsRanged = false};

            var sheet = new CharacterSheet();
            sheet.Strength = 12;
            sheet.Dexterity = 20;
            sheet.EquipedWeapon = weapon;
            Assert.AreEqual(1, sheet.GetCurrentDamageBonus());
            sheet.Strength = 14;
            sheet.Dexterity = 20;
            Assert.AreEqual(2, sheet.GetCurrentDamageBonus());
        }

        [Test]
        public void GetCurrentDamageBonusTest_Range()
        {
            var weapon = new Weapon() {IsRanged = true};

            var sheet = new CharacterSheet();
            sheet.Strength = 20;
            sheet.Dexterity = 12;
            sheet.EquipedWeapon = weapon;
            Assert.AreEqual(0, sheet.GetCurrentDamageBonus());
            sheet.Strength = 20;
            sheet.Dexterity = 14;
            Assert.AreEqual(0, sheet.GetCurrentDamageBonus());
        }

        [Test]
        public void GetCurrentInitiativeTest()
        {
            var sheet = new CharacterSheet();
            sheet.Dexterity = 0;
            Assert.AreEqual(-5, sheet.GetCurrentInitiative());
            sheet.Dexterity = 10;
            Assert.AreEqual(0, sheet.GetCurrentInitiative());
            sheet.Dexterity = 12;
            Assert.AreEqual(1, sheet.GetCurrentInitiative());
            sheet.Dexterity = 16;
            Assert.AreEqual(3, sheet.GetCurrentInitiative());
        }
    }
}
