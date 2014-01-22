﻿namespace DndTable.Core.Characters
{
    public interface ICharacterSheet
    {
        string Name { get; }

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
        int ArmorClass { get; }
        int Initiative { get; }

        int Speed { get; }
        int SizeModifier { get; }

        int BaseAttackBonus { get; }
        int MeleeAttackBonus { get; }
        int GetRangedAttackBonus(int range);

        IArmor EquipedArmor { get; }
        IWeapon EquipedWeapon { get; }


        int CurrentMeleeDamageBonus { get; }
        int CurrentRangeDamageBonus { get; }
    }
}