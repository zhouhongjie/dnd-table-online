using System;

namespace DndTable.Core.Entities
{
    internal abstract class BaseEntity : IEntity
    {
        #region IEntity Members

        private static int _idCounter = 1;

        private int _id = _idCounter++;
        public int Id { get { return _id; } }

        public abstract EntityTypeEnum EntityType { get; }

        public virtual bool IsBlocking { get { return true; } } 

        public Position Position { get; internal set; }

        // Dummy implement
        public double Angle { get { return 0.0; } }

        #endregion
    }
}
