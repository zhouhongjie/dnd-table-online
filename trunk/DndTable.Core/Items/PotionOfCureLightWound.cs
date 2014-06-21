using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;

namespace DndTable.Core.Items
{
    public class PotionOfCureLightWound : BasePotion
    {
        public override string Description
        {
            get { return "Cure light wound"; }
        }

        internal override bool Use(ICharacter character, IDiceRoller diceRoller)
        {
            var sheet = CharacterSheet.GetEditableSheet(character);

            sheet.HitPoints += diceRoller.Roll(character, DiceRollEnum.PotionEffect, 8, 1);

            if (sheet.HitPoints > sheet.MaxHitPoints)
                sheet.HitPoints = sheet.MaxHitPoints;

            return true;
        }
    }
}
