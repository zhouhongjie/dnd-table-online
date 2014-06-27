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

            var buff = diceRoller.Roll(character, DiceRollEnum.PotionEffect, 4, 1);
            sheet.DexterityAttribute.AddBuff(buff);

            return true;
        }

        public override int MaxRange
        {
            get { return 1; }
        }
    }
}
