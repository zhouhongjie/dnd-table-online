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
            var diceRoller = new DiceRoller();
            return new Game(board, diceRoller);
        }

        public static ICharacter CreateCharacter()
        {
            var sheet = new CharacterSheet();

            //var diceRoller = new DiceRoller();
            sheet.Strength = 10;
            sheet.Dexterity = 10;
            sheet.Constitution = 10;
            sheet.Intelligent = 10;
            sheet.Wisdom = 10;
            sheet.Charisma = 10;

            sheet.HitPoints = 10;
            sheet.ArmourClass = 10;

            return new Character(sheet);
        }

        public static IWeapon CreateWeapon()
        {
            var weapon = new Weapon();

            // Dagger
            weapon.DamageD = 4;

            return weapon;
        }
    }
}
