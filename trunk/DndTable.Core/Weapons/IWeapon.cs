using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    public enum WeaponProficiencyEnum
    {
        Simple, Martial, Exotic
    }

    public enum WeaponDamageTypeEnum
    {
        None, Piercing, Slashing, Bludgeoning
    }

    public interface IWeapon
    {
        #region Statistics

        WeaponProficiencyEnum Proficiency { get; }
        bool IsRanged { get; }

        int DamageD { get; }

        int CriticalMultiplier { get; }
        int CriticalRange { get; } // 0 = 20; 1 = 19-20; 2 = 18-20

        int RangeIncrement { get; }

        int Weight { get; }

        List<WeaponDamageTypeEnum> DamageTypes { get; }

        #endregion

        #region Operational status

        bool NeedsReload { get; }

        #endregion
    }
}
