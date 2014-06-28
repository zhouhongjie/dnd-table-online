using System;
using DndTable.Core.Armors;
using DndTable.Core.Dice;
using DndTable.Core.Entities;
using DndTable.Core.Items;
using DndTable.Core.Spells;
using DndTable.Core.Weapons;

namespace DndTable.Core.Characters
{
    internal class Character : BaseEntity, ICharacter
    {
        public ICharacterSheet CharacterSheet { get; private set; }
        public CharacterTypeEnum CharacterType { get; private set; }
        public override EntityTypeEnum EntityType { get { return EntityTypeEnum.Character; } }

        public Character(ICharacterSheet sheet, CharacterTypeEnum charType = CharacterTypeEnum.Unknown)
        {
            CharacterSheet = sheet;
            CharacterType = charType;
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

        public void GivePotion(IPotion potion)
        {
            Characters.CharacterSheet.GetEditableSheet(this).Potions.Add(potion);
        }

        public void GiveWeapon(IWeapon weapon)
        {
            Characters.CharacterSheet.GetEditableSheet(this).Weapons.Add(weapon);
        }

        public void PrepareSpell(ISpell spell)
        {
            var baseSpell = spell as BaseSpell;
            if (baseSpell == null)
                throw new ArgumentException();

            baseSpell.Caster = this;
            Characters.CharacterSheet.GetEditableSheet(this).Spells.Add(spell);
        }

    }
}
