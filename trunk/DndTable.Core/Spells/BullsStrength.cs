﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;

namespace DndTable.Core.Spells
{
    internal class BullsStrength : BaseSpell
    {
        public override string Description
        {
            get { return "Bull's strength"; }
        }

        public override bool CastOn(ICharacter character, IDiceRoller diceRoller)
        {
            var sheet = CharacterSheet.GetEditableSheet(character);

            var buff = diceRoller.Roll(Caster, DiceRollEnum.MagicEffect, 4, 1);
            var duration = 600; // 1hr / caster_lvl
            sheet.AddAndApplyEffect(new AttributeBuffEffect(sheet, duration, sheet.StrengthAttribute, buff));

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