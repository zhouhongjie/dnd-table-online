using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;

namespace DndTable.Core.Spells
{
    internal class CureLightWound : BaseSpell
    {
        public override string Description
        {
            get { return "Cure light wound"; }
        }

        public override int MaxRange
        {
            get { return 1; }
        }

        public override int Level
        {
            get { return 1; }
        }

        public override bool CastOn(ICharacter target, IDiceRoller diceRoller)
        {
            var sheet = CharacterSheet.GetEditableSheet(target);

            sheet.HitPoints += diceRoller.Roll(Caster, DiceRollEnum.MagicEffect, 8, 1);

            if (sheet.HitPoints > sheet.MaxHitPoints)
                sheet.HitPoints = sheet.MaxHitPoints;

            return true;
        }
    }
}
