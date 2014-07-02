using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;

namespace DndTable.Core.Spells
{
    // Own invention to test sleep condition (and didn't want to implement real 'sleep' spell)
    internal class SleepArrow : BaseSpell
    {
        public override string Description
        {
            get { return "Sleep arrow"; }
        }

        public override bool CastOn(ICharacter character, IDiceRoller diceRoller)
        {
            var casterSheet = CharacterSheet.GetEditableSheet(Caster);
            var targetSheet = CharacterSheet.GetEditableSheet(character);

            var dc = casterSheet.GetArcaneSpellDC(Level);

            if (!diceRoller.Check(character, DiceRollEnum.ResistEffect, 20, targetSheet.Will, dc))
            {
                var duration = 10; // 1 minute/caster_lvl
                targetSheet.AddAndApplyEffect(new SleepEffect(targetSheet, duration));
            }

            return true;
        }

        public override int MaxRange
        {
            get { return 110; } // = 100ft + 10ft/caster_lvl
        }

        public override int Level
        {
            get { return 1; }
        }

    }

    internal class SleepEffect : BaseEffect
    {
        internal SleepEffect(CharacterSheet sheet, int durationInRounds)
            : base(sheet, durationInRounds)
        {}

        internal override void Apply()
        {
            Sheet.EditableConditions.IsSleeping = true;
        }

        internal override bool CancelOnDamage { get { return true; } }
    }
}
