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

            weapon.Proficiency = WeaponProficiencyEnum.Simple;
            weapon.IsRanged = false;
            weapon.DamageD = 4;
            weapon.CriticalMultiplier = 2;
            weapon.CriticalRange = 1;
            weapon.RangeIncrement = 10;
            weapon.Weight = 1;
            weapon.DamageTypes.Add(WeaponDamageTypeEnum.Piercing);

            return weapon;
        }

        public static IWeapon CrossbowLight()
        {
            var weapon = new Weapon();

            weapon.Proficiency = WeaponProficiencyEnum.Simple;
            weapon.IsRanged = true;
            weapon.DamageD = 8;
            weapon.CriticalMultiplier = 2;
            weapon.CriticalRange = 1;
            weapon.RangeIncrement = 80;
            weapon.Weight = 6;
            weapon.DamageTypes.Add(WeaponDamageTypeEnum.Piercing);

            return weapon;
        }
    }
}
