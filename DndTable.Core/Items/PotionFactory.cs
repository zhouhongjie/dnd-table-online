using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core.Items
{
    public static class PotionFactory
    {
        public static IPotion CreatePotionOfCureLightWound()
        {
            return new PotionOfCureLightWound();
        }

        public static IPotion CreatePotionOfCatsGrace()
        {
            return new PotionOfCatsGrace();
        }

        public static IPotion CreatePotionOfBullsStrength()
        {
            return new PotionOfBullsStrength();
        }
    }
}
