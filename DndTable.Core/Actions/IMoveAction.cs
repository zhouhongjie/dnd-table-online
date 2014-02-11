using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core.Actions
{
    public interface IMoveAction : IAction
    {
        IAction Target(Position position);

        /// <summary>
        /// MaxRange in Tiles (not in feet)
        /// </summary>
        int MaxRange { get; }

        bool DoOneStep(Position newLocation);
    }
}
