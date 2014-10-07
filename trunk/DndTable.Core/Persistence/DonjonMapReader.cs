using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DndTable.Core.Entities;
using DndTable.Core.Factories;

namespace DndTable.Core.Persistence
{
    /// <summary>
    /// load TSV map exports from: http://donjon.bin.sh/
    /// </summary>
    public class DonjonMapReader
    {
        private string _dungeonFolder = @"../Data/Donjon/";

        public void SetDonjonFolder(string folder)
        {
            _dungeonFolder = folder;
        }

        internal bool LoadBoard(string name, out int maxX, out int maxY, out List<BaseEntity> entities)
        {
            // Init out params
            maxX = 0;
            maxY = 0;
            entities = null;

            // Check file
            var filename = _dungeonFolder + name + ".txt";
            if (!File.Exists(filename))
                return false;

            entities = new List<BaseEntity>();

            var allLines = File.ReadAllLines(filename);
            maxY = allLines.Length;

            for(int y=0; y < allLines.Length; y++)
            {
                var line = allLines[y];
                var cells = line.Split('\t');
                maxX = cells.Length;

                for (int x = 0; x < cells.Length; x++)
                {
                    var cell = cells[x];
                    BaseEntity newEntity = null;

                    // Walls
                    if (string.IsNullOrEmpty(cell))
                    {
                        newEntity = Factory.CreateEntity(EntityTypeEnum.Wall) as BaseEntity;
                    }

                    // Doors
                    if (cell.Contains("D"))
                    {
                        newEntity = Factory.CreateEntity(EntityTypeEnum.Door) as BaseEntity;
                    }

                    if (newEntity != null)
                    {
                        newEntity.Position = Position.Create(x, y);
                        entities.Add(newEntity);
                    }
                }
            }

            return true;
        }
    }
}
