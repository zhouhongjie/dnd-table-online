using DndTable.Core.Armors;
using DndTable.Core.Entities;
using DndTable.Core.Factories;
using DndTable.Core.Items;
using DndTable.Core.Weapons;

namespace DndTable.Core.Characters
{
    public interface ICharacter : IEntity
    {
        ICharacterSheet CharacterSheet { get; }
        CharacterTypeEnum CharacterType { get; }

        // Actions
        void EquipWeapon(IWeapon weapon);
        void EquipArmor(IArmor armor);

        // Inventory actions
        void GivePotion(IPotion potion);
        void GiveWeapon(IWeapon weapon);
    }

    public enum CharacterTypeEnum
    {
        Unknown, Hero, Orc, OrcChief
    }
}
