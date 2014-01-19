using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Actions;
using DndTable.Core.Characters;

namespace DndTable.Core
{
    public interface IEncounter
    {
        ICharacter GetCurrentCharacter();
        ICharacter GetNextCharacter();

        int GetRound();

        List<IAction> GetPossibleActionsForCurrentCharacter();
    }
}
