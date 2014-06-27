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
    }
}
