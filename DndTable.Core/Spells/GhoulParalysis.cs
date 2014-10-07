using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;

namespace DndTable.Core.Spells
{
    internal class GhoulParalysis : BaseSpell
    {
        public override string Description
        {
            get { return "Ghoul Paralysis"; }
        }

        public override bool CastOn(ICharacter character, IDiceRoller diceRoller)
        {
            var casterSheet = CharacterSheet.GetEditableSheet(Caster);
            var targetSheet = CharacterSheet.GetEditableSheet(character);

            var dc = 14;

            // TODO: elves are immune

            if (!diceRoller.Check(character, DiceRollEnum.ResistEffect, 20, targetSheet.Fortitude, dc))
            {
                // TODO: currently this is TOO EXTREME => need some way to wait when we count in minutes
                //var duration = diceRoller.Roll(character, DiceRollEnum.Duration, 6, 2) * 10; // 1D6+2 minutes
                var duration = diceRoller.Roll(character, DiceRollEnum.Duration, 6, 2); // 1D6+2 rounds
                targetSheet.AddAndApplyEffect(new GhoulParalysisEffect(targetSheet, duration));
            }

            return true;
        }

        public override int MaxRange
        {
            get { return 5; } // = 100ft + 10ft/caster_lvl
        }

        public override int Level
        {
            get { return 1; }
        }

    }

    internal class GhoulParalysisEffect : BaseEffect
    {
        internal GhoulParalysisEffect(CharacterSheet sheet, int durationInRounds)
            : base(sheet, durationInRounds)
        {}

        internal override void Apply()
        {
            Sheet.EditableConditions.IsParalyzed = true;
        }
    }
}
