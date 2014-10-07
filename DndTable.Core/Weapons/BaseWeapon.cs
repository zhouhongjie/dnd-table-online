using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;

namespace DndTable.Core.Weapons
{
    internal abstract class BaseWeapon : IWeapon
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


        // Abstract
        public abstract bool NeedsReload { get; }
        public abstract void Use();

        internal abstract void ApplyEffect(ICharacter target, IDiceRoller diceRoller);
    }
}
