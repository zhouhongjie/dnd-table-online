using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;

namespace DndTable.Core.Weapons
{
    internal class NaturalWeapon : IWeapon
    {
        public int Attack { get; private set; }


        public string Description { get; private set; }
        public WeaponProficiencyEnum Proficiency { get; private set; }
        public bool IsRanged { get; private set; }
        public int NrOfDamageDice { get; private set; }
        public int DamageD { get; private set; }
        public int CriticalMultiplier { get; private set; }
        public int CriticalRange { get; private set; }
        public int RangeIncrement { get; private set; }
        public int Weight { get; private set; }
        public List<WeaponDamageTypeEnum> DamageTypes { get; private set; }
        public bool NeedsReload { get; private set; }

        public void Use()
        {}

        public int DamageBonus { get; private set; }
        public bool ProvokesAoO { get; private set; }

        public NaturalWeapon(string name, bool isMelee, int attack, int damageRolls, int damageD, int damageBonus)
        {
            Description = name;
            Proficiency = WeaponProficiencyEnum.Natural;
            IsRanged = !isMelee;
            Attack = attack;
            NrOfDamageDice = damageRolls;
            DamageD = damageD;
            DamageBonus = damageBonus;

            // TODO: VERIFY???
            ProvokesAoO = false;

            // TODO
            CriticalMultiplier = 2;
            CriticalRange = 0;
            RangeIncrement = 0;
            DamageTypes = new List<WeaponDamageTypeEnum>() {WeaponDamageTypeEnum.Piercing};
            NeedsReload = false;
        }

        public int RollDamage(ICharacter roller, IDiceRoller diceRoller)
        {
            var damage = 0;

            for (var i = 0; i < NrOfDamageDice; i++)
            {
                damage += diceRoller.Roll(roller,
                                          DiceRollEnum.Damage,
                                          DamageD,
                                          DamageBonus);
            }

            return damage;
        }
    }
}
