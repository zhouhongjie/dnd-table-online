using System;
using System.Collections.Generic;
using DndTable.Core.Actions;
using DndTable.Core.Armors;
using DndTable.Core.Dice;
using DndTable.Core.Entities;
using DndTable.Core.Factories;
using DndTable.Core.Items;
using DndTable.Core.Persistence;
using DndTable.Core.Spells;
using DndTable.Core.Weapons;

namespace DndTable.Core.Characters
{
    internal class Character : BaseEntity, ICharacter
    {
        public ICharacterSheet CharacterSheet { get; private set; }
        public CharacterTypeEnum CharacterType { get; private set; }
        public override EntityTypeEnum EntityType { get { return EntityTypeEnum.Character; } }

        private Repository _repository { get; set; }

        public Character(ICharacterSheet sheet, CharacterTypeEnum charType = CharacterTypeEnum.Unknown)
        {
            CharacterSheet = sheet;
            CharacterType = charType;

            // TODO: use dependency injection
            _repository = Repository.CreateRepository();
        }

        public override bool IsBlocking
        {
            get { return CharacterSheet.CanAct(); }
        } 


        public void EquipWeapon(IWeapon weapon)
        {
            Characters.CharacterSheet.GetEditableSheet(this).EquipedWeapon = weapon;
        }

        public void EquipArmor(IArmor armor)
        {
            Characters.CharacterSheet.GetEditableSheet(this).EquipedArmor = armor;
        }

        public void Give(IPotion potion)
        {
            Characters.CharacterSheet.GetEditableSheet(this).Potions.Add(potion);
        }

        public void Give(IWeapon weapon)
        {
            Characters.CharacterSheet.GetEditableSheet(this).Weapons.Add(weapon);
        }

        public bool RemoveItem(IPotion potion)
        {
            return Characters.CharacterSheet.GetEditableSheet(this).Potions.Remove(potion);
        }

        public void PrepareSpell(ISpell spell)
        {
            var baseSpell = spell as BaseSpell;
            if (baseSpell == null)
                throw new ArgumentException();

            baseSpell.Caster = this;
            Characters.CharacterSheet.GetEditableSheet(this).Spells.Add(spell);
        }


        public bool SaveCharacterSheet(string name)
        {
            return _repository.SaveCharacterSheet(name, Characters.CharacterSheet.GetEditableSheet(this));
        }

        public bool LoadCharacterSheet(string name)
        {
            var sheet = Characters.CharacterSheet.GetEditableSheet(this);
            return _repository.LoadCharacterSheet(name, ref sheet);
        }


        internal override List<IAction> GetUseActions(ICharacter selectingCharacter, AbstractActionFactory actionFactory)
        {
            var actions = new List<IAction>();

            foreach (var potion in selectingCharacter.CharacterSheet.Potions)
            {
                var action = actionFactory.GiveItem(selectingCharacter, potion) as BaseAction;
                if (action == null)
                    throw new InvalidCastException();

                actions.Add(action.Target(this));
            }

            return actions;
        }

    }
}
