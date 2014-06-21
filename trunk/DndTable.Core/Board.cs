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

        private List<BaseEntity>[,] _cells;
        private bool _rebuildCells;

        private List<BaseEntity> _entities;
        private bool[,] _currentFieldOfView;

        private Repository _repository { get; set; }

        public Board(int maxX, int maxY)
        {
            Clear(maxX, maxY);
            
            // TODO: use dependency injection
            _repository = Repository.CreateRepository();
        }

        public List<IEntity> GetEntities()
        {
            return _entities.Cast<IEntity>().ToList();
        }

        private void Clear(int maxX, int maxY)
        {
            MaxX = maxX;
            MaxY = maxY;

            _entities = new List<BaseEntity>();

            _rebuildCells = true;
        }

        public bool Save(string name)
        {
            return _repository.SaveBoard(name, MaxX, MaxY, _entities);
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

        public IEntity GetEntity(Position position, EntityTypeEnum type)
        {
            if (_rebuildCells)
                RebuildOptimizedCells();

            var cell = _cells[position.X, position.Y];
            if (cell == null)
                return null;

            // TODO: possibly multiple entities that should be returned
            if (cell.Count(e => e.EntityType == type && e.Position == position) > 1)
                throw new NotSupportedException("multiple entities that should be returned");

            return cell.FirstOrDefault(e => e.EntityType == type && e.Position == position);
        }

        public List<IEntity> GetEntities(Position position)
        {
            if (_rebuildCells)
                RebuildOptimizedCells();

            var cell = _cells[position.X, position.Y];
            if (cell == null)
                return null;

            return cell.Cast<IEntity>().ToList();
        }

        private void RebuildOptimizedCells()
        {
            _cells = new List<BaseEntity>[MaxX, MaxY];

            foreach (var entity in _entities)
            {
                if (_cells[entity.Position.X, entity.Position.Y] == null)
                    _cells[entity.Position.X, entity.Position.Y] = new List<BaseEntity>();

                _cells[entity.Position.X, entity.Position.Y].Add(entity);
            }

            _rebuildCells = false;
        }

        public bool MoveEntity(IEntity entity, Position to)
        {
            if (!CheckBoundaries(to))
                return false;

            var baseEntity = entity as BaseEntity;
            if (baseEntity == null)
                throw new ArgumentException("Unsupported entity type");

            if (!_entities.Contains(baseEntity))
                throw new ArgumentException("Entity is not present on board");

            if (IsBlocked(to))
                return false;

            baseEntity.Position = to;

            _rebuildCells = true;

            return true;
        }

        // Is something blocking the way?
        private bool IsBlocked(Position to)
        {
            var entitiesOnTo = GetEntities(to);
            if (entitiesOnTo != null && entitiesOnTo.FirstOrDefault(e => e.IsBlocking) != null)
                return true;

            return false;
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

            foreach (var entity in _entities)
            {
                if (entity.EntityType == EntityTypeEnum.Wall)
                    map[entity.Position.X, entity.Position.Y] = true;
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

            var baseEntity = entity as BaseEntity;
            if (baseEntity == null)
                throw new ArgumentException();

            if (IsBlocked(position))
                return false;

            _entities.Add(baseEntity);
            baseEntity.Position = position;

            _rebuildCells = true;

            return true;
        }

        internal bool RemoveEntity(IEntity entity)
        {
            var baseEntity = entity as BaseEntity;
            if (baseEntity == null)
                throw new ArgumentException();

            if (!_entities.Contains(baseEntity))
                return false;

            _entities.Remove(baseEntity);

            _rebuildCells = true;

            return true;
        }
    }
}
