using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;

namespace DndTable.Core.Spells
{
    internal abstract class BaseSpell : ISpell
    {
        public ICharacter Executer { get; private set; }

        internal BaseSpell(ICharacter executer)
        {
            Executer = executer;
        }

        public abstract string Description { get; }
        public abstract int MaxRange { get; }

        public abstract bool CastOn(ICharacter target);

    }
}
