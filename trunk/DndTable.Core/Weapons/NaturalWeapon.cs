using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Spells;

namespace DndTable.Core.Weapons
{
    internal class NaturalWeapon : BaseWeapon
    {
        // Specific for natural weapons
        public int Attack { get; private set; }

        private readonly bool _needsReload;
        public override bool NeedsReload { get { return _needsReload; } }

        public BaseSpell Effect { get; private set; }

        public override void Use()
        {}

        internal override void ApplyEffect(ICharacter target, IDiceRoller diceRoller)
        {
            if (Effect == null)
                return;

            Effect.CastOn(target, diceRoller);
        }

        public int DamageBonus { get; private set; }

        public NaturalWeapon(string name, bool isMelee, int attack, int damageRolls, int damageD, int damageBonus, BaseSpell effect = null)
        {
            Description = name;
            Proficiency = WeaponProficiencyEnum.Natural;
            IsRanged = !isMelee;
            Attack = attack;
            NrOfDamageDice = damageRolls;
            DamageD = damageD;
            DamageBonus = damageBonus;
            Effect = effect;

            // TODO
            CriticalMultiplier = 2;
            CriticalRange = 0;
            RangeIncrement = 0;
            DamageTypes = new List<WeaponDamageTypeEnum>() {WeaponDamageTypeEnum.Piercing};
            _needsReload = false;
        }
    }
}
