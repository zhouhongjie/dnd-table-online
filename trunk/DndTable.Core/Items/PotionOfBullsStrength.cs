using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;

namespace DndTable.Core.Items
{
    internal class PotionOfBullsStrength : BasePotion
    {
        public override string Description
        {
            get { return "Bull's strength"; }
        }

        internal override bool Use(ICharacter character, IDiceRoller diceRoller)
        {
            var sheet = CharacterSheet.GetEditableSheet(character);

            var buff = diceRoller.Roll(character, DiceRollEnum.PotionEffect, 4, 1);
            if (buff > sheet.StrengthBuff)
                sheet.StrengthBuff = buff;

            return true;
        }
    }
}
