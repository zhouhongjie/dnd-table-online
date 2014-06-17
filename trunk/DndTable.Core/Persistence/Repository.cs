using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using DndTable.Core.Entities;

namespace DndTable.Core.Persistence
{
    internal class Repository
    {
        private string _folder { get; set; }

        public static Repository CreateRepository()
        {
            // TODO check default Data folder path
            return new Repository(@".\Data\");
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

        //internal bool LoadBoard(Board board, string name)
        //{
        //    return false;
        //}
    }
}
