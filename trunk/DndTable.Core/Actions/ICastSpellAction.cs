using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Spells;

namespace DndTable.Core.Actions
{
    public interface ICastSpellAction
    {
        IAction Target(ICharacter character);

        /// <summary>
        /// MaxRange in Tiles (not in feet)
        /// </summary>
        int MaxRange { get; }

        ISpell Spell { get; }
    }
}
