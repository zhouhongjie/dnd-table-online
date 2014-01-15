using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core.Factories
{
    public static class WeaponFactory
    {
        public static IWeapon Dagger()
        {
            var weapon = new Weapon();

            // Dagger
            weapon.DamageD = 4;

            return weapon;
        }
    }
}
