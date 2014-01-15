using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    public static class Factory
    {
        public static IGame CreateGame(int maxX, int maxY)
        {
            var board = new Board(maxX, maxY);
            var diceRoller = new DiceRoller(new DiceRandomizer());
            return new Game(board, diceRoller);
        }

        public static ICharacter CreateCharacter(string name)
        {
            var sheet = new CharacterSheet();

            sheet.Name = name;

            //var diceRoller = new DiceRoller();
            sheet.Strength = 10;
            sheet.Dexterity = 10;
            sheet.Constitution = 10;
            sheet.Intelligent = 10;
            sheet.Wisdom = 10;
            sheet.Charisma = 10;

            sheet.HitPoints = 10;

            return new Character(sheet);
        }

        public static IWeapon CreateWeapon()
        {
            var weapon = new Weapon();

            // Dagger
            weapon.DamageD = 4;

            return weapon;
        }

        public static IArmor CreateArmor()
        {
            var armour = new Armor();

            // Leather armour
            armour.ArmorBonus = 2;

            return armour;
        }
    }
}
