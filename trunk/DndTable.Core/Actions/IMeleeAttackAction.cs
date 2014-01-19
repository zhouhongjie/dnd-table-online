using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;

namespace DndTable.Core.Actions
{
    public interface IMeleeAttackAction : IAction
    {
        IAction Target(ICharacter character);
    }
}
