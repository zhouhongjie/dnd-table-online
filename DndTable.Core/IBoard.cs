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

        IEntity GetEntity(int x, int y);
    }
}
