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

            armour.Proficiency = ArmorProficiencyEnum.Light;
            armour.ArmorBonus = 2;
            armour.MaxDexBonus = 6;
            armour.ArmorCheckPenalty = 0;
            armour.ArcaneSpellFailure = 10;

            return armour;
        }

        public static IArmor ScaleMail()
        {
            var armour = new Armor();

            armour.Proficiency = ArmorProficiencyEnum.Medium;
            armour.ArmorBonus = 4;
            armour.MaxDexBonus = 3;
            armour.ArmorCheckPenalty = 4;
            armour.ArcaneSpellFailure = 25;

            return armour;
        }
    }
}
