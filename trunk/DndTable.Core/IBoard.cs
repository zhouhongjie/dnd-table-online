using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    public interface IBoard
    {
        int MaxX { get; }
        int MaxY { get; }

        IEntity GetEntity(Position position);
        bool MoveEntity(Position from, Position to);
    }
}
