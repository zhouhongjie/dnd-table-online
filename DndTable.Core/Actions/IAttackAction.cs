using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;

namespace DndTable.Core.Actions
{
    public interface IAttackAction : IAction
    {
        IAction Target(ICharacter character);

        /// <summary>
        /// MaxRange in Tiles (not in feet)
        /// </summary>
        int MaxRange { get; }

        /// <summary>
        /// MaxRange in Tiles (not in feet)
        /// </summary>
        int MinRange { get; }
    }
}
