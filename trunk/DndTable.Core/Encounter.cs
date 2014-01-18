using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    class Encounter : IEncounter
    {
        public Characters.ICharacter GetCurrentCharacter()
        {
            throw new NotImplementedException();
        }

        public void GetPossibleActionsForCurrentCharacter()
        {
            throw new NotImplementedException();
        }
    }
}
