using DndTable.Core.Armors;
using DndTable.Core.Entities;
using DndTable.Core.Factories;
using DndTable.Core.Items;
using DndTable.Core.Spells;
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

        void PrepareSpell(ISpell spell);

        // Inventory actions
        void GivePotion(IPotion potion);
        void GiveWeapon(IWeapon weapon);

        // Persistence
        bool SaveCharacterSheet(string name);
        bool LoadCharacterSheet(string name);
    }

    public enum CharacterTypeEnum
    {
        Unknown, Hero, Orc, OrcChief, Kobolt
    }
}
