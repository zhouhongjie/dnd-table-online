using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    public class CharacterSheet : ICharacterSheet
    {
        public string Name { get; internal set; }

        public int Strength { get; internal set; }
        public int Dexterity { get; internal set; }

        public int Constitution { get; internal set; }

        public int Intelligent { get; internal set; }

        public int Wisdom { get; internal set; }

        public int Charisma { get; internal set; }

        public int Fortitude { get; internal set; }

        public int Reflex { get; internal set; }

        public int Will { get; internal set; }

        public int HitPoints { get; internal set; }

        public int ArmourClass
        {
            get
            {
                // TODO ... armour, bonusses, touch, flatfooted, ...
                return 10 + GetAbilityBonus(Dexterity);
            }
        }

        public int Initiative { get; internal set; }

        public int Speed { get; internal set; }

        public int BaseAttackBonus { get; internal set; }

        public int MeleeAttackBonus { get; internal set; }

        public int RangedAttackBonus { get; internal set; }

        public IArmour EquipedArmour { get; internal set; }

        public IWeapon EquipedWeapon { get; internal set; }

        public int CurrentMeleeDamageBonus
        {
            get
            {
                // TODO: weapon bonus
                // TODO: weapon focus
                // ...

                return GetAbilityBonus(Strength);
            }
        }

        private static int GetAbilityBonus(int baseAbiltyScore)
        {
            return (int)Math.Floor((baseAbiltyScore - 10) / 2.0);
        }
    }
}
