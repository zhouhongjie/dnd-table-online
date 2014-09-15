using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;

namespace DndTable.Core.Spells
{
    internal class CatsGrace : BaseSpell
    {
        public override string Description
        {
            get { return "Cat's grace"; }
        }

        public override bool CastOn(ICharacter character, IDiceRoller diceRoller)
        {
            var sheet = CharacterSheet.GetEditableSheet(character);

            var buffValue = diceRoller.Roll(Caster, DiceRollEnum.MagicEffect, 4, 1);
            var duration = 600; // 1hr / caster_lvl
            //sheet.AddAndApplyEffect(new AttributeBuffEffect(sheet, duration, sheet.DexterityAttribute, buff));


            // NEW
            {
                // Spell creates an effect
                // Effect creates AttributeBuff
                // AttributeBuff influences attribute

                var effect = new AttributeBuffEffect2(sheet, duration);
                var buff = new AttributeBuff(effect, buffValue);

                sheet.DexterityAttribute.AddBuff(buff);
                sheet.AddAndApplyEffect(effect);
            }

            return true;
        }

        public override int MaxRange
        {
            get { return 1; }
        }

        public override int Level
        {
            get { return 2; }
        }
    }
}
