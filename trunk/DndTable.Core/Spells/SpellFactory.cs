using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;

namespace DndTable.Core.Spells
{
    public static class SpellFactory
    {
        public static ISpell MagicMissile()
        {
            return new MagicMissile();
        }

        public static ISpell CureLightWound()
        {
            return new CureLightWound();
        }
        public static ISpell CatsGrace()
        {
            return new CatsGrace();
        }

        public static ISpell BullsStrength()
        {
            return new BullsStrength();
        }
    }
}
