using System.Collections.Generic;
using DndTable.Core.Items;

namespace DndTable.Core.Weapons
{
    public enum WeaponProficiencyEnum
    {
        Simple, Martial, Exotic, Natural
    }

    public enum WeaponDamageTypeEnum
    {
        None, Piercing, Slashing, Bludgeoning
    }

    public interface IWeapon : IItem
    {
        #region Statistics

        WeaponProficiencyEnum Proficiency { get; }
        bool IsRanged { get; }

        int NrOfDamageDice { get; }
        int DamageD { get; }

        int CriticalMultiplier { get; }
        int CriticalRange { get; } // 0 = 20; 1 = 19-20; 2 = 18-20

        int RangeIncrement { get; }

        int Weight { get; }

        List<WeaponDamageTypeEnum> DamageTypes { get; }

        bool ProvokesAoO { get; }

        #endregion

        #region Operational status

        bool NeedsReload { get; }

        #endregion

        #region operation

        void Use();

        #endregion
    }
}
