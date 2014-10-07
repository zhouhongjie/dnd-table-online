using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;

namespace DndTable.Core.Spells
{
    internal class SpecialAbilityFactory
    {
        public static BaseSpell GhoulParalysis(ICharacter executer)
        {
            var special = new GhoulParalysis();
            special.Caster = executer;
            return special;
        }
    }
}
