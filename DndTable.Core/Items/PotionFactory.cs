using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Spells;

namespace DndTable.Core.Items
{
    public static class PotionFactory
    {
        public static IPotion CreatePotionOfCureLightWound()
        {
            return new SpellEffectPotion(SpellFactory.CureLightWound() as BaseSpell);
        }

        public static IPotion CreatePotionOfCatsGrace()
        {
            return new SpellEffectPotion(SpellFactory.CatsGrace() as BaseSpell);
        }

        public static IPotion CreatePotionOfBullsStrength()
        {
            return new SpellEffectPotion(SpellFactory.BullsStrength() as BaseSpell);
        }
    }
}
