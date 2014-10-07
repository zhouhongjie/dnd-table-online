using DndTable.Core.Actions;

namespace DndTable.Core.Weapons
{
    public static class WeaponFactory
    {
        public static IWeapon Dagger()
        {
            var weapon = new Weapon();

            weapon.Description = "Dagger";
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

            weapon.Description = "Longsword";
            weapon.Proficiency = WeaponProficiencyEnum.Martial;
            weapon.IsRanged = false;
            weapon.DamageD = 8;
            weapon.CriticalMultiplier = 2;
            weapon.CriticalRange = 1;
            weapon.RangeIncrement = 0;
            weapon.Weight = 4;
            weapon.DamageTypes.Add(WeaponDamageTypeEnum.Slashing);

            return weapon;
        }

        public static IWeapon BattleAxe()
        {
            var weapon = new Weapon();

            weapon.Description = "Battle axe";
            weapon.Proficiency = WeaponProficiencyEnum.Martial;
            weapon.IsRanged = false;
            weapon.DamageD = 8;
            weapon.CriticalMultiplier = 3;
            weapon.CriticalRange = 1;
            weapon.RangeIncrement = 0;
            weapon.Weight = 7;
            weapon.DamageTypes.Add(WeaponDamageTypeEnum.Slashing);

            return weapon;
        }

        public static IWeapon MaceLight()
        {
            var weapon = new Weapon();

            weapon.Description = "Light mace";
            weapon.Proficiency = WeaponProficiencyEnum.Simple;
            weapon.IsRanged = false;
            weapon.DamageD = 6;
            weapon.CriticalMultiplier = 2;
            weapon.CriticalRange = 0;
            weapon.RangeIncrement = 0;
            weapon.Weight = 6;
            weapon.DamageTypes.Add(WeaponDamageTypeEnum.Bludgeoning);

            return weapon;
        }

        public static IWeapon MaceHeavy()
        {
            var weapon = new Weapon();

            weapon.Description = "Heavy mace";
            weapon.Proficiency = WeaponProficiencyEnum.Simple;
            weapon.IsRanged = false;
            weapon.DamageD = 8;
            weapon.CriticalMultiplier = 2;
            weapon.CriticalRange = 0;
            weapon.RangeIncrement = 0;
            weapon.Weight = 12;
            weapon.DamageTypes.Add(WeaponDamageTypeEnum.Bludgeoning);

            return weapon;
        }

        public static IWeapon Rapier()
        {
            var weapon = new Weapon();

            weapon.Description = "Rapier";
            weapon.Proficiency = WeaponProficiencyEnum.Martial;
            weapon.IsRanged = false;
            weapon.DamageD = 6;
            weapon.CriticalMultiplier = 2;
            weapon.CriticalRange = 2;
            weapon.RangeIncrement = 0;
            weapon.Weight = 3;
            weapon.DamageTypes.Add(WeaponDamageTypeEnum.Piercing);

            return weapon;
        }

        public static IWeapon CrossbowLight()
        {
            var weapon = new Weapon();

            weapon.Description = "Light crossbow";
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

        public static IWeapon CrossbowHeavy()
        {
            var weapon = new Weapon();

            weapon.Description = "Heavy crossbow";
            weapon.Proficiency = WeaponProficiencyEnum.Simple;
            weapon.IsRanged = true;
            weapon.DamageD = 10;
            weapon.CriticalMultiplier = 2;
            weapon.CriticalRange = 2;
            weapon.RangeIncrement = 120;
            weapon.Weight = 9;
            weapon.DamageTypes.Add(WeaponDamageTypeEnum.Piercing);

            weapon.ReloadInfo = new ReloadInfo()
                                {
                                    IsLoaded = true,
                                    ActionType = ActionTypeEnum.FullRound
                                };

            return weapon;
        }

        public static IWeapon Longbow()
        {
            var weapon = new Weapon();

            weapon.Description = "Longbow";
            weapon.Proficiency = WeaponProficiencyEnum.Martial;
            weapon.IsRanged = true;
            weapon.DamageD = 6;
            weapon.CriticalMultiplier = 3;
            weapon.CriticalRange = 0;
            weapon.RangeIncrement = 60;
            weapon.Weight = 2;
            weapon.DamageTypes.Add(WeaponDamageTypeEnum.Piercing);

            return weapon;
        }

        public static IWeapon Shortbow()
        {
            var weapon = new Weapon();

            weapon.Description = "Shortbow";
            weapon.Proficiency = WeaponProficiencyEnum.Martial;
            weapon.IsRanged = true;
            weapon.DamageD = 8;
            weapon.CriticalMultiplier = 3;
            weapon.CriticalRange = 0;
            weapon.RangeIncrement = 100;
            weapon.Weight = 3;
            weapon.DamageTypes.Add(WeaponDamageTypeEnum.Piercing);

            return weapon;
        }

        public static IWeapon Club()
        {
            var weapon = new Weapon();

            weapon.Description = "Club";
            weapon.Proficiency = WeaponProficiencyEnum.Simple;
            weapon.IsRanged = false;
            weapon.DamageD = 6;
            weapon.CriticalMultiplier = 2;
            weapon.CriticalRange = 0;
            weapon.RangeIncrement = 10;
            weapon.Weight = 3;
            weapon.DamageTypes.Add(WeaponDamageTypeEnum.Bludgeoning);

            return weapon;
        }

        public static IWeapon HalfSpear()
        {
            // (*) Can be used by small character!

            var weapon = new Weapon();

            weapon.Description = "Halfspear";
            weapon.Proficiency = WeaponProficiencyEnum.Simple;
            weapon.IsRanged = false;
            weapon.DamageD = 6;
            weapon.CriticalMultiplier = 3;
            weapon.CriticalRange = 0;
            weapon.RangeIncrement = 20;
            weapon.Weight = 3; // ?
            weapon.DamageTypes.Add(WeaponDamageTypeEnum.Piercing);

            return weapon;
        }
    }
}
