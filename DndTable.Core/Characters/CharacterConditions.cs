using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core.Characters
{
    internal class CharacterConditions : ICharacterConditions
    {
        public bool IsSleeping { get; internal set; }

        public bool IsHelpless
        {
            get { return IsSleeping; } // TODO: extend with unconsious, bound, paralyzed, ...
        }

        internal void ClearEffects()
        {
            // TODO: work with ConditionAttributes, where we clear the IsSleeping.EffectValue (but leave the IsSleeping.Value)
            // => otherwise a creature can never sleep naturally
            IsSleeping = false;
        }
    }
}
