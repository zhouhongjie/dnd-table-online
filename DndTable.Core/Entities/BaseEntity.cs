using System;
using System.Collections.Generic;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Factories;

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

        internal virtual List<IAction> GetUseActions(ICharacter character, AbstractActionFactory actionFactory)
        {
            return null;
        }

        #endregion
    }
}
