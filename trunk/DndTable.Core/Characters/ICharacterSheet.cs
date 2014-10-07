using System.Collections.Generic;
using DndTable.Core.Armors;
using DndTable.Core.Items;
using DndTable.Core.Spells;
using DndTable.Core.Weapons;

namespace DndTable.Core.Characters
{
    public interface ICharacterSheet
    {
        string Name { get; }
        CharacterRace Race { get; }
        int FactionId { get; }

        Dictionary<CharacterClass, int> Level { get; }

        int Strength { get; }
        int Dexterity { get; }
        int Constitution { get; }
        int Intelligence { get; }
        int Wisdom { get; }
        int Charisma { get; }

        int Fortitude { get; }
        int Reflex { get; }
        int Will { get; }

        int HitPoints { get; }
        int MaxHitPoints { get; }

        int Speed { get; }
        int SizeModifier { get; }
        SizeEnum Size { get; }

        int BaseAttackBonus { get; }

        IArmor EquipedArmor { get; }
        IWeapon EquipedWeapon { get; }

        List<IPotion> Potions { get; }
        List<IWeapon> Weapons { get; }
        List<ISpell> Spells { get; }

        ICharacterConditions Conditions { get; }
        ICharacterImmunities Immunities { get; }

        bool HasNaturalWeapons { get; }

        bool CanAct();
        int GetCurrentAttackBonus(int range, bool isFlanking);
        //int GetCurrentDamageBonus();
        int GetCurrentSpeed();
        int GetCurrentArmorClass();
        int GetCurrentInitiative();

        IWeapon GetCurrentWeapon();
    }

    public enum SizeEnum
    {
        Small, Medium, Large
    }

}
