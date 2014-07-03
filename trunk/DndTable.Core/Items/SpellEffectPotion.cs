using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Spells;

namespace DndTable.Core.Items
{
    internal class SpellEffectPotion : BasePotion
    {
        private BaseSpell _spellEffect;

        internal SpellEffectPotion(BaseSpell spell)
        {
            _spellEffect = spell;
        }

        public override string Description
        {
            get { return _spellEffect.Description; }
        }

        internal override bool Use(Characters.ICharacter character, Dice.IDiceRoller diceRoller)
        {
            // TODO: not entirely correct => caster can also be used to determine spell effectiveness (caster lvl, ...)
            // Should be the caster that created the potion (or a dummy caster)
            _spellEffect.Caster = character;
            return _spellEffect.CastOn(character, diceRoller);
        }
    }
}
