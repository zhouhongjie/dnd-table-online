using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;

namespace DndTable.Core.Items
{
    public abstract class BasePotion : IPotion
    {
        public abstract string Description { get; }

        internal abstract bool Use(ICharacter character, IDiceRoller diceRoller);
    }
}
