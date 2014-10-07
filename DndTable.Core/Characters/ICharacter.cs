﻿using DndTable.Core.Armors;
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
        void Give(IItem item);
        bool RemoveItem(IItem item);

        // Feats (TODO: create feats as classes)
        void AddSneakAttackFeat();

        // Persistence
        bool SaveCharacterSheet(string name);
        bool LoadCharacterSheet(string name);
    }

    public enum CharacterTypeEnum
    {
        Unknown, Hero, Orc, OrcChief, Kobolt, Wolf, MediumSkeleton, MediumZombie, Ghoul
    }
}
