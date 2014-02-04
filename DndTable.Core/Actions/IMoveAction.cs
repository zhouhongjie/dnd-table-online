using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core.Actions
{
    public interface IMoveAction : IAction
    {
        IAction Target(Position position);

        bool DoOneStep(Position newLocation);
    }
}
