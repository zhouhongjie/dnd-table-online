using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;

namespace DndTable.Core.Spells
{
    internal class MagicMissile : BaseSpell
    {
        public override string Description { get { return "Magic missile"; } }

        // Medium (100 ft. + 10 ft./level)
        public override int MaxRange { get { return 100; } }
        public override int Level { get { return 1; } }

        public override bool CastOn(ICharacter target, IDiceRoller diceRoller)
        {
            // TODO: evolve by lvl

            var damage = diceRoller.Roll(Caster, DiceRollEnum.MagicEffect, 4, 1);

            // TODO: spell resistance => here or in CastSpell action?

            CharacterSheet.GetEditableSheet(target).ApplyDamage(damage);

            return true;
        }
    }
}
