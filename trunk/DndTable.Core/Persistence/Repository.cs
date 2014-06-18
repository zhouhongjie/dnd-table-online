using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DndTable.Core.Entities;
using DndTable.Core.Factories;

namespace DndTable.Core.Persistence
{
    internal class Repository
    {
        private string _folder { get; set; }

        public static Repository CreateRepository()
        {
            // TODO check default Data folder path
            //return new Repository(@".\Data\");
            return new Repository(@"E:\Data\Projects\DndTableOnline\Data\");
        }

        internal Repository(string folder)
        {
            _folder = folder;
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

                // Don't save characters
                if (entity.EntityType == EntityTypeEnum.Character)
                    continue;

                var newEntityXml = new EntityXml()
                                       {
                                           EntityType = entity.EntityType,
                                           PositionX = entity.Position.X,
                                           PositionY = entity.Position.Y
                                       };
                boardXml.Entities.Add(newEntityXml);
            }


            using (var writeFileStream = new StreamWriter(_folder + name + ".xml"))
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
            var filename = _folder + name + ".xml";
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
                    if (entityXml.EntityType != EntityTypeEnum.Wall)
                        throw new NotSupportedException("EntityType not supported yet: " + entityXml.EntityType);

                    var newEntity = Factory.CreateWall() as BaseEntity;
                    newEntity.Position = Position.Create(entityXml.PositionX, entityXml.PositionY);
                    entities.Add(newEntity);
                }

                return true;
            }
        }
    }
}
