using System.Collections.Generic;
using DndTable.Core.Actions;

namespace DndTable.Core.Weapons
{
    internal class Weapon : IWeapon
    {
        public string Description { get; internal set; }
        public int NrOfDamageDice { get; internal set; }
        public int DamageD { get; internal set; }
        public WeaponProficiencyEnum Proficiency { get; internal set; }
        public bool IsRanged { get; internal set; }
        public int CriticalMultiplier { get; internal set; }
        public int CriticalRange { get; internal set; }
        public int RangeIncrement { get; internal set; }
        public int Weight { get; internal set; }
        public List<WeaponDamageTypeEnum> DamageTypes { get; internal set; }

        public bool ProvokesAoO { get { return IsRanged; } }


        public bool NeedsReload { get { return this.ReloadInfo != null && !this.ReloadInfo.IsLoaded; } }
        public ReloadInfo ReloadInfo { get; internal set; }

        internal Weapon()
        {
            NrOfDamageDice = 1;
            CriticalMultiplier = 1;
            Proficiency = WeaponProficiencyEnum.Simple;
            DamageTypes = new List<WeaponDamageTypeEnum>();
        }

        public void Use()
        {
            if (ReloadInfo != null)
            {
                ReloadInfo.IsLoaded = false;
            }
        }
    }

    internal class ReloadInfo
    {
        public bool IsLoaded { get; internal set; }
        public ActionTypeEnum ActionType { get; internal set; }
    }
}
