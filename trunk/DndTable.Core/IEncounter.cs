using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Entities;

namespace DndTable.Core
{
    public interface IEncounter
    {
        List<ICharacter> Participants { get; }

        ICharacter GetCurrentCharacter();
        ICharacter GetNextCharacter();

        int GetRound();

        List<IAction> GetPossibleActionsForCurrentCharacter();

        bool SetEntityContext(IEntity entity);
    }
}
