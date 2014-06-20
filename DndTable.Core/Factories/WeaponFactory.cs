using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Actions;

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

        public static IWeapon Longsword()
        {
            var weapon = new Weapon();

            weapon.Proficiency = WeaponProficiencyEnum.Martial;
            weapon.IsRanged = false;
            weapon.DamageD = 8;
            weapon.CriticalMultiplier = 2;
            weapon.CriticalRange = 2;
            weapon.RangeIncrement = 0;
            weapon.Weight = 4;
            weapon.DamageTypes.Add(WeaponDamageTypeEnum.Slashing);

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

            weapon.ReloadInfo = new ReloadInfo()
                                {
                                    IsLoaded = true,
                                    ActionType = ActionTypeEnum.MoveEquivalent
                                };

            return weapon;
        }

        public static IWeapon Longbow()
        {
            var weapon = new Weapon();

            weapon.Proficiency = WeaponProficiencyEnum.Martial;
            weapon.IsRanged = true;
            weapon.DamageD = 8;
            weapon.CriticalMultiplier = 3;
            weapon.CriticalRange = 1;
            weapon.RangeIncrement = 100;
            weapon.Weight = 3;
            weapon.DamageTypes.Add(WeaponDamageTypeEnum.Piercing);

            return weapon;
        }

        public static IWeapon Club()
        {
            var weapon = new Weapon();

            weapon.Proficiency = WeaponProficiencyEnum.Simple;
            weapon.IsRanged = false;
            weapon.DamageD = 6;
            weapon.CriticalMultiplier = 2;
            weapon.CriticalRange = 0;
            weapon.RangeIncrement = 10;
            weapon.Weight = 6; // ?
            weapon.DamageTypes.Add(WeaponDamageTypeEnum.Bludgeoning);

            return weapon;
        }
    }
}
