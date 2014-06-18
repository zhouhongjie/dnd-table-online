using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Entities;
using DndTable.Core.Persistence;
using SilverlightShadowCasting;

namespace DndTable.Core
{
    internal class Board : IBoard
    {
        public int MaxX { get; private set; }
        public int MaxY { get; private set; }

        private BaseEntity[,] _cells;
        private bool[,] _currentFieldOfView;

        private Repository _repository { get; set; }

        public Board(int maxX, int maxY)
        {
            Clear(maxX, maxY);
            
            // TODO: use dependency injection
            _repository = Repository.CreateRepository();
        }

        private void Clear(int maxX, int maxY)
        {
            MaxX = maxX;
            MaxY = maxY;

            _cells = new BaseEntity[MaxX, MaxY];
        }

        public bool Save(string name)
        {
            var entityList = new List<BaseEntity>();
            for (int i = 0; i < _cells.GetLength(0); i++)
            {
                for (int j=0; j < _cells.GetLength(1); j++)
                {
                    var entity = _cells[i, j];
                    if (entity != null)
                        entityList.Add(entity);
                }
            }

            return _repository.SaveBoard(name, MaxX, MaxY, entityList);
        }

        public bool Load(string name)
        {
            int maxX, maxY;
            List<BaseEntity> entities;

            if (!_repository.LoadBoard(name, out maxX, out maxY, out entities))
                return false;

            Clear(maxX, maxY);

            foreach (var entity in entities)
            {
                AddEntity(entity, entity.Position);
            }

            return true;
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

        public bool IsVisibleForCurrentPlayer(Position position)
        {
            if (_currentFieldOfView == null)
                throw new InvalidOperationException("CalculateFieldOfView should have been called before using IsVisibleForCurrentPlayer");

            return _currentFieldOfView[position.X, position.Y];
        }

        public bool[,] GetFieldOfViewForCurrentPlayer()
        {
            if (_currentFieldOfView == null)
                throw new InvalidOperationException("CalculateFieldOfView should have been called before using IsVisibleForCurrentPlayer");

            return _currentFieldOfView;
        }

        public bool[,] GetFieldOfView(Position origin)
        {
            return CalculateFieldOfView(origin);
        }

        internal void OptimizeFieldOfViewForCurrentPlayer(Position origin)
        {
            _currentFieldOfView = CalculateFieldOfView(origin);
        }

        private bool[,] CalculateFieldOfView(Position origin)
        {
            bool[,] map = new bool[MaxX, MaxY];

            // Create outerwalls
            for (var i = 0; i < MaxX; i++)
            {
                map[i, 0] = true;
                map[i, MaxY - 1] = true;
            }
            for (var i = 0; i < MaxY; i++)
            {
                map[0, i] = true;
                map[MaxX - 1, i] = true;
            }

            // Add walls
            for (var i = 0; i < _cells.GetLength(0); i++)
            {
                for (var j = 0; j < _cells.GetLength(1); j++)
                {
                    if (_cells[i, j] != null)
                        map[i, j] = _cells[i, j].EntityType == EntityTypeEnum.Wall;
                }
            }

            // Calculate FoV
            int radius = 1000; // infinite
            bool[,] fieldOfView = new bool[MaxX, MaxY];
            ShadowCaster.ComputeFieldOfViewWithShadowCasting(
                origin.X, origin.Y, radius,
                (x1, y1) => map[x1, y1],
                (x2, y2) => { fieldOfView[x2, y2] = true; });

            return fieldOfView;
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

        internal bool RemoveEntity(Position position)
        {
            if (!CheckBoundaries(position))
                return false;

            if (_cells[position.X, position.Y] == null)
                return false;

            _cells[position.X, position.Y] = null;

            return true;
        }
    }
}
