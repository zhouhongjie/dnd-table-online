using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DndTable.Core.Characters;
using DndTable.Core.Entities;
using DndTable.Core.Factories;

namespace DndTable.Core.Persistence
{
    internal class Repository
    {
        private string _dungeonFolder { get; set; }
        private string _characterSheetFolder { get; set; }

        public static Repository CreateRepository()
        {
            // TODO check default Data folder path
            //return new Repository(@".\Data\");

            var dungeonFolder = @"../Data/Maps/";
            var characterSheetFolder = @"../Data/Characters/";

            

            return new Repository(dungeonFolder, characterSheetFolder);
        }

        internal Repository(string dungeonFolder, string characterSheetFolder)
        {
            _dungeonFolder = dungeonFolder;
            _characterSheetFolder = characterSheetFolder;
        }

        internal bool SaveCharacterSheet(string name, CharacterSheet sheet)
        {
            var sheetXml = new CharacterSheetXml();
            sheetXml.CopyFrom(sheet);

            using (var writeFileStream = new StreamWriter(_characterSheetFolder + name + ".xml"))
            {
                var serializer = new XmlSerializer(typeof(CharacterSheetXml));
                serializer.Serialize(writeFileStream, sheetXml);
            }

            return true;            
        }

        internal bool LoadCharacterSheet(string name, ref CharacterSheet sheet)
        {
            var filename = _characterSheetFolder + name + ".xml";
            if (!File.Exists(filename))
                return false;

            // Read file
            using (var readFileStream = new StreamReader(filename))
            {
                var serializer = new XmlSerializer(typeof(CharacterSheetXml));
                var sheetXml = serializer.Deserialize(readFileStream) as CharacterSheetXml;

                if (sheetXml == null)
                {
                    return false;
                }

                sheetXml.CopyTo(sheet);
            }

            return true;
        }

        internal bool SaveBoard(string name, int maxX, int maxY, List<BaseEntity> entities)
        {
            var boardXml = new BoardXml();
            boardXml.MaxX = maxX;
            boardXml.MaxY = maxY;

            boardXml.Entities = new List<EntityXml>();
            foreach (var entity in entities)
            {
                if (entity == null)
                    throw new ArgumentNullException();


                var newEntityXml = new EntityXml()
                                       {
                                           EntityType = entity.EntityType,
                                           PositionX = entity.Position.X,
                                           PositionY = entity.Position.Y
                                       };

                var character = entity as ICharacter;
                if (character != null)
                {
                    // Don't save hero characters
                    if (character.CharacterType == CharacterTypeEnum.Hero)
                        continue;

                    newEntityXml.CharacterType = character.CharacterType;
                }

               boardXml.Entities.Add(newEntityXml);
            }


            using (var writeFileStream = new StreamWriter(_dungeonFolder + name + ".xml"))
            {
                var serializer = new XmlSerializer(typeof(BoardXml));
                serializer.Serialize(writeFileStream, boardXml);
            }

            return true;
        }

        internal bool LoadBoard(string name, out int maxX, out int maxY, out List<BaseEntity> entities)
        {
            // Init out params
            maxX = 0;
            maxY = 0;
            entities = null;

            // Check file
            var filename = _dungeonFolder + name + ".xml";
            if (!File.Exists(filename))
                return false;

            // Read file
            using (var readFileStream = new StreamReader(filename))
            {
                var serializer = new XmlSerializer(typeof(BoardXml));
                var boardXml = serializer.Deserialize(readFileStream) as BoardXml;

                if (boardXml == null)
                {
                    return false;
                }

                maxX = boardXml.MaxX;
                maxY = boardXml.MaxY;

                entities = new List<BaseEntity>();
                foreach (var entityXml in boardXml.Entities)
                {
                    BaseEntity newEntity;

                    if (entityXml.EntityType == EntityTypeEnum.Wall)
                    {
                        newEntity = Factory.CreateWall() as BaseEntity;
                    }
                    else if (entityXml.EntityType == EntityTypeEnum.Chest)
                    {
                        newEntity = Factory.CreateChest() as BaseEntity;
                    }
                    else if (entityXml.EntityType == EntityTypeEnum.Door)
                    {
                        newEntity = Factory.CreateDoor() as BaseEntity;
                    }
                    else if (entityXml.EntityType == EntityTypeEnum.Character)
                    {
                        newEntity = Factory.CreateNpc(entityXml.CharacterType) as BaseEntity;
                    }
                    else
                    {
                        throw new NotSupportedException("EntityType not supported yet: " + entityXml.EntityType);
                    }

                    newEntity.Position = Position.Create(entityXml.PositionX, entityXml.PositionY);
                    entities.Add(newEntity);

                }

                return true;
            }
        }
    }
}
