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

        public static ICharacter CreateCharacter(string name, int str, int dex, int con, int intel, int wis, int cha)
        {
            var sheet = new CharacterSheet();

            sheet.Name = name;
            sheet.FactionId = 1;

            sheet.Strength = str;
            sheet.Dexterity = dex;
            sheet.Constitution = con;
            sheet.Intelligence = intel;
            sheet.Wisdom = wis;
            sheet.Charisma = cha;

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
            if (npcType == CharacterTypeEnum.Kobolt)
                return CreateKobolt();

            throw new NotImplementedException();
        }

        public static ICharacter CreateOrc(string name = "Orc")
        {
            var sheet = new CharacterSheet();

            sheet.Name = name;
            sheet.Race = CharacterRace.Orc;
            sheet.FactionId = 2;

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

        public static ICharacter CreateKobolt(string name = "Kobolt")
        {
            var sheet = new CharacterSheet();

            sheet.Name = name;
            sheet.Race = CharacterRace.Reptilian;
            sheet.FactionId = 2;

            sheet.Strength = 6;
            sheet.Dexterity = 13;
            sheet.Constitution = 11;
            sheet.Intelligence = 10;
            sheet.Wisdom = 10;
            sheet.Charisma = 10;

            sheet.HitPoints = 2;
            sheet.MaxHitPoints = 2;
            sheet.Speed = 30;

            sheet.SizeModifier = 1;

            sheet.NaturalArmor = 1;

            sheet.EquipedWeapon = WeaponFactory.CrossbowLight();
            sheet.EquipedArmor = ArmorFactory.Leather();
            sheet.Weapons.Add(WeaponFactory.HalfSpear());

            return new Character(sheet, CharacterTypeEnum.Kobolt);
        }

        //public static ICharacter CreateWolf(string name = "Wolf")
        //{
        //    var sheet = new CharacterSheet();

        //    sheet.Name = name;
        //    sheet.Race = CharacterRace.Animal;
        //    sheet.FactionId = 2;

        //    sheet.Strength = 13;
        //    sheet.Dexterity = 15;
        //    sheet.Constitution = 15;
        //    sheet.Intelligence = 2;
        //    sheet.Wisdom = 12;
        //    sheet.Charisma = 6;

        //    sheet.HitPoints = 13;
        //    sheet.MaxHitPoints = 13;
        //    sheet.Speed = 50;

        //    sheet.NaturalArmor = 2;

        //    // TODO... ???? bite..
        //    //sheet.EquipedWeapon = WeaponFactory.CrossbowLight();
        //    //sheet.EquipedArmor = ArmorFactory.Leather();
        //    //sheet.Weapons.Add(WeaponFactory.HalfSpear());

        //    return new Character(sheet, CharacterTypeEnum.Kobolt);
        //}

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
