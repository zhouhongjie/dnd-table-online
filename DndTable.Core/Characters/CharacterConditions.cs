using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Log;

namespace DndTable.Core.Characters
{
    internal class CharacterConditions : ICharacterConditions
    {
        private CharacterSheet _sheet;

        internal CharacterConditions(CharacterSheet sheet)
        {
            _sheet = sheet;
        }

        private bool _isSleeping;
        public bool IsSleeping
        {
            get { return _isSleeping;  }
            set
            {
                if (value && _sheet.Immunities.ImmuneToSleep)
                    Logger.Singleton.LogImmunity(_sheet, "ToSleep");
                else
                    _isSleeping = value;
            }
        }

        public bool IsFlatFooted { get; internal set; }

        public bool IsHelpless
        {
            get { return IsSleeping; } // TODO: extend with unconsious, bound, paralyzed, ...
        }

        public bool CanDoOnlyPartialActions { get; internal set; }

        // REMOVE ASAP!!!
        internal void ClearEffects()
        {
            // VERIFY!!!!

            // TODO: work with ConditionAttributes, where we clear the IsSleeping.EffectValue (but leave the IsSleeping.Value)
            // => otherwise a creature can never sleep naturally
            IsSleeping = false;
        }
    }
}
