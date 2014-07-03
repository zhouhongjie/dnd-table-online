using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;

namespace DndTable.Core.Weapons
{
    internal class NaturalWeapon
    {
        public string Name { get; private set; }
        public bool IsMelee { get; private set; }
        public int Attack { get; private set; }
        public int DamageRolls { get; private set; }
        public int DamageD { get; private set; }
        public int DamageBonus { get; private set; }

        public NaturalWeapon(string name, bool isMelee, int attack, int damageRolls, int damageD, int damageBonus)
        {
            Name = name;
            IsMelee = isMelee;
            Attack = attack;
            DamageRolls = damageRolls;
            DamageD = damageD;
            DamageBonus = damageBonus;
        }

        public int RollDamage(ICharacter roller, IDiceRoller diceRoller)
        {
            var damage = 0;

            for (var i = 0; i < DamageRolls; i++)
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
