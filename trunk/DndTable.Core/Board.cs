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

        private readonly BaseEntity[,] _cells;

        public Board(int maxX, int maxY)
        {
            MaxX = maxX;
            MaxY = maxY;

            _cells = new BaseEntity[MaxX, MaxY];
        }

        private bool CheckBoundaries(Position position)
        {
            if (position.X >= MaxX)
                return false;
            if (position.Y >= MaxY)
                return false;
            return true;
        }

        public IEntity GetEntity(Position position)
        {
            if (!CheckBoundaries(position))
                return null;

            return _cells[position.X, position.Y];
        }

        public bool MoveEntity(Position from, Position to)
        {
            if (!CheckBoundaries(from))
                return false;
            if (!CheckBoundaries(to))
                return false;

            if (GetEntity(to) != null)
                return false;

            var entity = _cells[from.X, from.Y];
            if (entity == null)
                return false;

            _cells[from.X, from.Y] = null;
            _cells[to.X, to.Y] = entity;

            entity.Position = to;

            return true;
        }

        internal bool AddEntity(IEntity entity, Position position)
        {
            if (!CheckBoundaries(position))
                return false;

            if (_cells[position.X, position.Y] != null)
                return false;

            var baseEntity = entity as BaseEntity;
            if (baseEntity == null)
                throw new ArgumentException();

            _cells[position.X, position.Y] = baseEntity;
            baseEntity.Position = position;

            return true;
        }
    }
}
