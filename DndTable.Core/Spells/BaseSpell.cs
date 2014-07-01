using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;

namespace DndTable.Core.Spells
{
    internal abstract class BaseSpell : ISpell
    {
        public ICharacter Caster { get; internal set; }

        public abstract string Description { get; }
        public abstract int MaxRange { get; }
        public abstract int Level { get; }

        public abstract bool CastOn(ICharacter target, IDiceRoller diceRoller);

    }
}
