using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Entities;

namespace DndTable.Core.Factories
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
            sheet.Speed = 30;

            return new Character(sheet);
        }

        public static ICharacter CreateOrc()
        {
            var sheet = new CharacterSheet();

            sheet.Name = "Orc";
            sheet.Race = CharacterRace.Orc;

            //var diceRoller = new DiceRoller();
            sheet.Strength = 15;
            sheet.Dexterity = 10;
            sheet.Constitution = 11;
            sheet.Intelligent = 9;
            sheet.Wisdom = 8;
            sheet.Charisma = 8;

            sheet.HitPoints = 4;
            sheet.Speed = 30;

            sheet.EquipedWeapon = WeaponFactory.Club();
            sheet.EquipedArmor = ArmorFactory.ScaleMail();

            return new Character(sheet);
        }

        public static ICharacter CreateCharacter(string name, ICharacterSheet sheet)
        {
            return new Character(sheet);
        }

        public static IEntity CreateWall()
        {
            return new Wall();
        }
    }
}
