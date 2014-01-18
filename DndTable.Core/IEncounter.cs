using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;

namespace DndTable.Core
{
    public interface IEncounter
    {
        ICharacter GetCurrentCharacter();
        void GetPossibleActionsForCurrentCharacter();
    }
}
