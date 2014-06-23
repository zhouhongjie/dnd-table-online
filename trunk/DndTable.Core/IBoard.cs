using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Entities;

namespace DndTable.Core
{
    public interface IBoard
    {
        int MaxX { get; }
        int MaxY { get; }

        List<IEntity> GetEntities();

        IEntity GetEntity(Position position, EntityTypeEnum type);
        List<IEntity> GetEntities(Position position);
        bool MoveEntity(IEntity entity, Position to);

        bool IsVisibleForCurrentPlayer(Position origin);
        bool[,] GetFieldOfViewForCurrentPlayer();

        bool[,] GetFieldOfView(Position origin);

        bool Save(string name);
        bool Load(string name);
    }
}
