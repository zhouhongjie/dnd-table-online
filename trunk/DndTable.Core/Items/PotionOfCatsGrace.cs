using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;

namespace DndTable.Core.Items
{
    internal class PotionOfCatsGrace : BasePotion
    {
        public override string Description
        {
            get { return "Cat's grace"; }
        }

        internal override bool Use(ICharacter character, IDiceRoller diceRoller)
        {
            var sheet = CharacterSheet.GetEditableSheet(character);

            var buff = diceRoller.Roll(character, DiceRollEnum.PotionEffect, 4, 1);
            sheet.DexterityAttribute.AddBuff(buff);

            return true;
        }
    }
}
