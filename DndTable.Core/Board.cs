using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    internal class Board : IBoard
    {
        public int MaxX { get; private set; }
        public int MaxY { get; private set; }

        private readonly IEntity[,] _cells;

        public Board(int maxX, int maxY)
        {
            MaxX = maxX;
            MaxY = maxY;

            _cells = new IEntity[MaxX, MaxY];
        }

        public IEntity GetEntity(int x, int y)
        {
            if (x >= MaxX)
                return null;
            if (y >= MaxY)
                return null;

            return _cells[x, y];
        }

        internal bool AddEntity(IEntity entity, int x, int y)
        {
            if (x >= MaxX)
                return false;
            if (y >= MaxY)
                return false;

            if (_cells[x, y] != null)
                return false;

            _cells[x, y] = entity;
            return true;
        }
    }
}
