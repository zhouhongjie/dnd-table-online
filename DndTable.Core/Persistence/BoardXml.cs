﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Entities;

namespace DndTable.Core.Persistence
{
    [Serializable()]
    public class BoardXml
    {
        public int MaxX { get; set; }
        public int MaxY { get; set; }

        public List<EntityXml> Entities { get; set; }
    }

    [Serializable()]
    public class EntityXml
    {
        public EntityTypeEnum EntityType { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
    }
}
