using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    internal class Weapon : IWeapon
    {
        public int DamageD { get; internal set; }
        public WeaponProficiencyEnum Proficiency { get; internal set; }
        public bool IsRanged { get; internal set; }
        public int CriticalMultiplier { get; internal set; }
        public int CriticalRange { get; internal set; }
        public int RangeIncrement { get; internal set; }
        public int Weight { get; internal set; }
        public List<WeaponDamageTypeEnum> DamageTypes { get; internal set; }
        

        internal Weapon()
        {
            CriticalMultiplier = 1;
            Proficiency = WeaponProficiencyEnum.Simple;
            DamageTypes = new List<WeaponDamageTypeEnum>();
        }
    }
}
