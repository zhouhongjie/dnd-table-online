using System;
using DndTable.Core.Armors;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Entities;
using DndTable.Core.Weapons;

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
            sheet.FactionId = 1;

            //var diceRoller = new DiceRoller();
            sheet.Strength = 10;
            sheet.Dexterity = 10;
            sheet.Constitution = 10;
            sheet.Intelligence = 10;
            sheet.Wisdom = 10;
            sheet.Charisma = 10;

            sheet.HitPoints = 10;
            sheet.MaxHitPoints = 10;
            sheet.Speed = 30;

            return new Character(sheet, CharacterTypeEnum.Hero);
        }

        public static ICharacter CreateCharacter(string name, int strength, int dexterity)
        {
            var sheet = new CharacterSheet();

            sheet.Name = name;
            sheet.FactionId = 1;

            //var diceRoller = new DiceRoller();
            sheet.Strength = strength;
            sheet.Dexterity = dexterity;
            sheet.Constitution = 10;
            sheet.Intelligence = 10;
            sheet.Wisdom = 10;
            sheet.Charisma = 10;

            sheet.HitPoints = 10;
            sheet.MaxHitPoints = 10;
            sheet.Speed = 30;

            return new Character(sheet, CharacterTypeEnum.Hero);
        }

        public static ICharacter CreateNpc(CharacterTypeEnum npcType)
        {
            if (npcType == CharacterTypeEnum.Orc)
                return CreateOrc();
            if (npcType == CharacterTypeEnum.OrcChief)
                return CreateOrcChief();

            throw new NotImplementedException();
        }

        public static ICharacter CreateOrc(string name = "Orc")
        {
            var sheet = new CharacterSheet();

            sheet.Name = name;
            sheet.Race = CharacterRace.Orc;
            sheet.FactionId = 2;

            //var diceRoller = new DiceRoller();
            sheet.Strength = 15;
            sheet.Dexterity = 10;
            sheet.Constitution = 11;
            sheet.Intelligence = 9;
            sheet.Wisdom = 8;
            sheet.Charisma = 8;

            sheet.HitPoints = 4;
            sheet.MaxHitPoints = 4;
            sheet.Speed = 30;

            sheet.EquipedWeapon = WeaponFactory.Club();
            sheet.EquipedArmor = ArmorFactory.ScaleMail();

            return new Character(sheet, CharacterTypeEnum.Orc);
        }

        public static ICharacter CreateOrcChief(string name = "Orc Chief")
        {
            var sheet = new CharacterSheet();

            sheet.Name = name;
            sheet.Race = CharacterRace.Orc;
            sheet.FactionId = 2;

            //var diceRoller = new DiceRoller();
            sheet.Strength = 16;
            sheet.Dexterity = 10;
            sheet.Constitution = 12;
            sheet.Intelligence = 9;
            sheet.Wisdom = 8;
            sheet.Charisma = 8;

            sheet.HitPoints = 9;
            sheet.MaxHitPoints = 9;
            sheet.Speed = 30;

            sheet.EquipedWeapon = WeaponFactory.BattleAxe();
            sheet.EquipedArmor = ArmorFactory.ScaleMail();

            return new Character(sheet, CharacterTypeEnum.OrcChief);
        }

        public static IEntity CreateWall()
        {
            return new Wall();
        }

        public static IEntity CreateChest()
        {
            return new Chest();
        }

        public static IEntity CreateDoor()
        {
            return new Door();
        }
    }
}
