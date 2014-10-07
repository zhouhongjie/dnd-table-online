using System;
using DndTable.Core.Armors;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Entities;
using DndTable.Core.Spells;
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

        private static CharacterSheet CreateDefaultSheet()
        {
            var sheet = new CharacterSheet();

            sheet.Strength = 10;
            sheet.Dexterity = 10;
            sheet.Constitution = 10;
            sheet.Intelligence = 10;
            sheet.Wisdom = 10;
            sheet.Charisma = 10;

            sheet.HitPoints = 10;
            sheet.MaxHitPoints = 10;
            sheet.Speed = 30;

            sheet.Size = SizeEnum.Medium;

            return sheet;
        }

        private static CharacterSheet CreateSheet(string name, int str, int dex, int con, int intel, int wis, int cha, SizeEnum size)
        {
            var sheet = CreateDefaultSheet();

            sheet.Name = name;

            sheet.Strength = str;
            sheet.Dexterity = dex;
            sheet.Constitution = con;
            sheet.Intelligence = intel;
            sheet.Wisdom = wis;
            sheet.Charisma = cha;

            sheet.Size = size;

            return sheet;
        }

        public static ICharacter CreateCharacter(string name)
        {
            var sheet = CreateDefaultSheet();

            sheet.Name = name;
            sheet.FactionId = 1;

            return new Character(sheet, CharacterTypeEnum.Hero);
        }

        public static ICharacter CreateCharacter(string name, int str, int dex, int con, int intel, int wis, int cha, SizeEnum size = SizeEnum.Medium)
        {
            var sheet = CreateSheet(name, str, dex, con, intel, wis, cha, size);
            sheet.FactionId = 1;
            return new Character(sheet, CharacterTypeEnum.Hero);
        }

        public static ICharacter CreateNpcCharacter(string name, int str, int dex, int con, int intel, int wis, int cha, SizeEnum size = SizeEnum.Medium)
        {
            var sheet = CreateSheet(name, str, dex, con, intel, wis, cha, size);

            // TODO: better faction differences
            sheet.FactionId = 3; 

            return new Character(sheet, CharacterTypeEnum.Npc);
        }

        public static ICharacter CreateNpc(CharacterTypeEnum npcType)
        {
            if (npcType == CharacterTypeEnum.Orc)
                return CreateOrc();
            if (npcType == CharacterTypeEnum.OrcChief)
                return CreateOrcChief();
            if (npcType == CharacterTypeEnum.Kobolt)
                return CreateKobolt();
            if (npcType == CharacterTypeEnum.Wolf)
                return CreateWolf();
            if (npcType == CharacterTypeEnum.MediumSkeleton)
                return CreateMediumSkeleton();
            if (npcType == CharacterTypeEnum.MediumZombie)
                return CreateMediumZombie();
            if (npcType == CharacterTypeEnum.Ghoul)
                return CreateGhoul();

            throw new NotImplementedException();
        }

        public static ICharacter CreateOrc(string name = "Orc")
        {
            var sheet = CreateDefaultSheet();

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
            var sheet = CreateDefaultSheet();

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
            var sheet = CreateDefaultSheet();

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

            sheet.Size = SizeEnum.Small;

            sheet.NaturalArmor = 1;

            sheet.EquipedWeapon = WeaponFactory.CrossbowLight();
            sheet.EquipedArmor = ArmorFactory.Leather();
            sheet.Weapons.Add(WeaponFactory.HalfSpear());

            return new Character(sheet, CharacterTypeEnum.Kobolt);
        }

        public static ICharacter CreateWolf(string name = "Wolf")
        {
            var sheet = CreateDefaultSheet();

            sheet.Name = name;
            sheet.Race = CharacterRace.Animal;
            sheet.FactionId = 2;

            sheet.Strength = 13;
            sheet.Dexterity = 15;
            sheet.Constitution = 15;
            sheet.Intelligence = 2;
            sheet.Wisdom = 12;
            sheet.Charisma = 6;

            sheet.HitPoints = 13;
            sheet.MaxHitPoints = 13;
            sheet.Speed = 50;

            sheet.NaturalArmor = 2;
            sheet.NaturalWeapons.Add(new NaturalWeapon("Bite", true, 3, 1, 6, 1));

            // TODO: Special attack => Trip

            return new Character(sheet, CharacterTypeEnum.Wolf);
        }

        public static ICharacter CreateMediumSkeleton(string name = "Skeleton")
        {
            var sheet = CreateDefaultSheet();

            sheet.Name = name;
            sheet.Race = CharacterRace.Undead;
            sheet.FactionId = 2;

            sheet.Strength = 10;
            sheet.Dexterity = 12;
            //sheet.Constitution = 10;
            //sheet.Intelligence = 9;
            sheet.Wisdom = 10;
            sheet.Charisma = 11;

            sheet.HitPoints = 6;
            sheet.MaxHitPoints = 6;
            sheet.Speed = 30;

            sheet.ImprovedInitiative = true;

            sheet.NaturalArmor = 2;
            sheet.NaturalWeapons.Add(new NaturalWeapon("Claw", true, 2, 1, 4, 0));
            sheet.NaturalWeapons.Add(new NaturalWeapon("Claw", true, 2, 1, 4, 0));

            ImmunityBuilder.AddUndeadImmunities(sheet.EditableImmunities);
            sheet.EditableImmunities.ImmuneToCold = true;
            sheet.EditableImmunities.HalfDamageFromPiercing = true;
            sheet.EditableImmunities.HalfDamageFromSlashing = true;

            return new Character(sheet, CharacterTypeEnum.MediumSkeleton);
        }

        public static ICharacter CreateMediumZombie(string name = "Zombie")
        {
            var sheet = CreateDefaultSheet();

            sheet.Name = name;
            sheet.Race = CharacterRace.Undead;
            sheet.FactionId = 2;

            sheet.Strength = 13;
            sheet.Dexterity = 8;
            //sheet.Constitution = 10;
            //sheet.Intelligence = 10;
            sheet.Wisdom = 10;
            sheet.Charisma = 1;

            sheet.HitPoints = 16;
            sheet.MaxHitPoints = 16;
            sheet.Speed = 30;

            sheet.NaturalArmor = 2;
            sheet.NaturalWeapons.Add(new NaturalWeapon("Slam", true, 2, 1, 6, 1));

            ImmunityBuilder.AddUndeadImmunities(sheet.EditableImmunities);

            sheet.EditableConditions.CanDoOnlyPartialActions = true;

            return new Character(sheet, CharacterTypeEnum.MediumZombie);
        }

        public static ICharacter CreateGhoul(string name = "Ghoul")
        {
            var sheet = CreateDefaultSheet();
            var character = new Character(sheet, CharacterTypeEnum.Ghoul);

            sheet.Name = name;
            sheet.Race = CharacterRace.Undead;
            sheet.FactionId = 2;

            sheet.Strength = 13;
            sheet.Dexterity = 15;
            //sheet.Constitution = 10;
            sheet.Intelligence = 13;
            sheet.Wisdom = 14;
            sheet.Charisma = 16;

            sheet.HitPoints = 13;
            sheet.MaxHitPoints = 13;
            sheet.Speed = 30;

            sheet.NaturalArmor = 2;
            sheet.NaturalWeapons.Add(new NaturalWeapon("Bite", true, 3, 1, 6, 1, SpecialAbilityFactory.GhoulParalysis(character)));
            sheet.NaturalWeapons.Add(new NaturalWeapon("Claw", true, 0, 1, 3, 0, SpecialAbilityFactory.GhoulParalysis(character)));
            sheet.NaturalWeapons.Add(new NaturalWeapon("Claw", true, 0, 1, 3, 0, SpecialAbilityFactory.GhoulParalysis(character)));

            ImmunityBuilder.AddUndeadImmunities(sheet.EditableImmunities);

            return character;
        }

        public static IEntity CreateEntity(EntityTypeEnum entityType)
        {
            IEntity newEntity = null;

            if (entityType == EntityTypeEnum.Wall)
            {
                newEntity = Factory.CreateWall() as BaseEntity;
            }
            else if (entityType == EntityTypeEnum.Chest)
            {
                newEntity = Factory.CreateChest() as BaseEntity;
            }
            else if (entityType == EntityTypeEnum.Door)
            {
                newEntity = Factory.CreateDoor() as BaseEntity;
            }
            else if (entityType == EntityTypeEnum.Pit)
            {
                newEntity = Factory.CreatePit() as BaseEntity;
            }
            else if (entityType == EntityTypeEnum.Character)
            {
                throw new NotSupportedException("use CreateEntity(EntityTypeEnum entityType, CharacterTypeEnum characterType)");
            }
            else
            {
                throw new NotImplementedException("EntityType not supported yet: " + entityType);
            }

            return newEntity;
        }

        public static IEntity CreateEntity(EntityTypeEnum entityType, CharacterTypeEnum characterType)
        {
            var newEntity = 
                entityType == EntityTypeEnum.Character 
                ? CreateNpc(characterType) 
                : CreateEntity(entityType);

            return newEntity;
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

        public static IEntity CreatePit()
        {
            return new Pit();
        }
    }
}
