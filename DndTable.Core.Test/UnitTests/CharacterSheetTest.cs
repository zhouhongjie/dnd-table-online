using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Armors;
using DndTable.Core.Characters;
using DndTable.Core.Factories;
using DndTable.Core.Weapons;
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
            sheet.DexterityAttribute.SetValue(0);
            Assert.AreEqual(5, sheet.GetCurrentArmorClass());
            sheet.DexterityAttribute.SetValue(10);
            Assert.AreEqual(10, sheet.GetCurrentArmorClass());
            sheet.DexterityAttribute.SetValue(20);
            Assert.AreEqual(15, sheet.GetCurrentArmorClass());
        }

        [Test]
        public void ArmorClassArmorTest()
        {
            var sheet = new CharacterSheet();
            sheet.DexterityAttribute.SetValue(10);

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
            sheet.DexterityAttribute.SetValue(10);

            // Size
            sheet.Size = SizeEnum.Small;
            Assert.AreEqual(11, sheet.GetCurrentArmorClass());

            sheet.Size = SizeEnum.Large;
            Assert.AreEqual(9, sheet.GetCurrentArmorClass());
        }

        [Test]
        public void GetCurrentAttackBonusTest_Melee()
        {
            var weapon = new Weapon() { IsRanged = false };

            var sheet = new CharacterSheet();
            sheet.StrengthAttribute.SetValue(12);
            sheet.DexterityAttribute.SetValue(20);
            sheet.EquipedWeapon = weapon;
            Assert.AreEqual(1, sheet.GetCurrentAttackBonus(5, false));
            sheet.StrengthAttribute.SetValue(14);
            sheet.DexterityAttribute.SetValue(20);
            Assert.AreEqual(2, sheet.GetCurrentAttackBonus(5, false));
        }

        [Test]
        public void GetCurrentAttackBonusTest_Ranged()
        {
            var weapon = new Weapon() {IsRanged = true, RangeIncrement = 100};

            // Test Dex
            var sheet = new CharacterSheet();
            sheet.StrengthAttribute.SetValue(20);
            sheet.DexterityAttribute.SetValue(12);
            sheet.EquipedWeapon = weapon;
            Assert.AreEqual(1, sheet.GetCurrentAttackBonus(10, false));
            sheet.StrengthAttribute.SetValue(20);
            sheet.DexterityAttribute.SetValue(14);
            Assert.AreEqual(2, sheet.GetCurrentAttackBonus(10, false));

            // Test RangeIncrement
            Assert.AreEqual(2, sheet.GetCurrentAttackBonus(99, false));
            Assert.AreEqual(0, sheet.GetCurrentAttackBonus(100, false));
            Assert.AreEqual(0, sheet.GetCurrentAttackBonus(199, false));
            Assert.AreEqual(-2, sheet.GetCurrentAttackBonus(200, false));
            Assert.AreEqual(-2, sheet.GetCurrentAttackBonus(299, false));
            Assert.AreEqual(-4, sheet.GetCurrentAttackBonus(300, false));
        }

        [Test]
        public void GetCurrentAttackBonusTest_Flanking()
        {
            var sheet = new CharacterSheet();
            sheet.StrengthAttribute.SetValue(10);
            sheet.DexterityAttribute.SetValue(10);

            // Unarmed
            Assert.AreEqual(0, sheet.GetCurrentAttackBonus(5, false));
            Assert.AreEqual(2, sheet.GetCurrentAttackBonus(5, true));

            // Melee weapon
            sheet.EquipedWeapon = new Weapon() { IsRanged = false };
            Assert.AreEqual(2, sheet.GetCurrentAttackBonus(5, true));

            // Ranged weapon
            sheet.EquipedWeapon = new Weapon() { IsRanged = true };
            Assert.AreEqual(0, sheet.GetCurrentAttackBonus(5, true));
        }

        [Test]
        public void GetCurrentDamageBonusTest_Melee()
        {
            var weapon = new Weapon() {IsRanged = false};

            var sheet = new CharacterSheet();
            sheet.StrengthAttribute.SetValue(12);
            sheet.DexterityAttribute.SetValue(20);
            sheet.EquipedWeapon = weapon;
            Assert.AreEqual(1, sheet.GetCurrentDamageRoll().Bonus);
            sheet.StrengthAttribute.SetValue(14);
            sheet.DexterityAttribute.SetValue(20);
            Assert.AreEqual(2, sheet.GetCurrentDamageRoll().Bonus);
        }

        [Test]
        public void GetCurrentDamageBonusTest_Range()
        {
            var weapon = new Weapon() {IsRanged = true};

            var sheet = new CharacterSheet();
            sheet.StrengthAttribute.SetValue(20);
            sheet.DexterityAttribute.SetValue(12);
            sheet.EquipedWeapon = weapon;
            Assert.AreEqual(0, sheet.GetCurrentDamageRoll().Bonus);
            sheet.StrengthAttribute.SetValue(20);
            sheet.DexterityAttribute.SetValue(14);
            Assert.AreEqual(0, sheet.GetCurrentDamageRoll().Bonus);
        }

        [Test]
        public void GetCurrentInitiativeTest()
        {
            var sheet = new CharacterSheet();
            sheet.DexterityAttribute.SetValue(0);
            Assert.AreEqual(-5, sheet.GetCurrentInitiative());
            sheet.DexterityAttribute.SetValue(10);
            Assert.AreEqual(0, sheet.GetCurrentInitiative());
            sheet.DexterityAttribute.SetValue(12);
            Assert.AreEqual(1, sheet.GetCurrentInitiative());
            sheet.DexterityAttribute.SetValue(16);
            Assert.AreEqual(3, sheet.GetCurrentInitiative());
        }
    }
}
