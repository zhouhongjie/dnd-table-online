using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    public interface IEntity
    {
        EntityTypeEnum EntityType { get; }
        Position Position { get; }
        double Angle { get; }
    }

    public enum EntityTypeEnum
    {
        Character
    }
}
