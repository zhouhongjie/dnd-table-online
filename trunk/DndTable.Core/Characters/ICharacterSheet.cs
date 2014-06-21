using System.Collections.Generic;
using DndTable.Core.Armors;
using DndTable.Core.Items;
using DndTable.Core.Weapons;

namespace DndTable.Core.Characters
{
    public interface ICharacterSheet
    {
        string Name { get; }
        CharacterRace Race { get; }
        int FactionId { get; }

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
        int MaxHitPoints { get; }

        int Speed { get; }
        int SizeModifier { get; }

        int BaseAttackBonus { get; }

        IArmor EquipedArmor { get; }
        IWeapon EquipedWeapon { get; }

        List<IPotion> Potions { get; }


            bool CanAct();
        int GetCurrentAttackBonus(int range, bool isFlanking);
        int GetCurrentDamageBonus();
        int GetCurrentSpeed();
        int GetCurrentArmorClass();
        int GetCurrentInitiative();
    }
}
