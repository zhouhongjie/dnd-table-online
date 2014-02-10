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

            sheet.Name = "orc";

            //var diceRoller = new DiceRoller();
            sheet.Strength = 12;
            sheet.Dexterity = 10;
            sheet.Constitution = 12;
            sheet.Intelligent = 8;
            sheet.Wisdom = 10;
            sheet.Charisma = 8;

            sheet.HitPoints = 10;
            sheet.Speed = 30;

            sheet.EquipedWeapon = WeaponFactory.Club();
            sheet.EquipedArmor = ArmorFactory.Leather();

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
