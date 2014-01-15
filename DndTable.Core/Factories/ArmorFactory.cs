using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core.Factories
{
    public static class ArmorFactory
    {
        public static IArmor Leather()
        {
            var armour = new Armor();

            // Leather armour
            armour.ArmorBonus = 2;

            return armour;
        }
    }
}
