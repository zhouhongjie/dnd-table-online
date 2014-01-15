using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    public interface ICharacterSheet
    {
        int Strength { get; }
        int Dexterity { get; }
        int Constitution { get; }
        int Intelligent { get; }
        int Wisdom { get; }
        int Charisma { get; }

        int Fortitude { get; }
        int Reflex { get; }
        int Will { get; }

        int HitPoints { get; }
        int ArmourClass { get; }
        int Initiative { get; }

        int Speed { get; }

        int BaseAttackBonus { get; }
        int MeleeAttackBonus { get; }
        int RangedAttackBonus { get; }

        IArmour EquipedArmour { get; }
        IWeapon EquipedWeapon { get; }


        int CurrentMeleeDamageBonus { get; }
    }
}
