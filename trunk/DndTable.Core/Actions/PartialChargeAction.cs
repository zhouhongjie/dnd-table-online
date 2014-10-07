using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;

namespace DndTable.Core.Actions
{
    class PartialChargeAction : ChargeAction
    {
        internal PartialChargeAction(ICharacter attacker)
            : base(attacker)
        {}

        public override int MaxRange
        {
            get
            {
                return _attacker.CharacterSheet.GetCurrentSpeed() / 5 + 1; // speed + 1 for attack (/5 for ft)
            }
        }

        public override ActionTypeEnum Type
        {
            // TO VERIFY: OK FOR PARTIAL CHARGE
            // As a follow-up, this action triggers an attack = Move + Attack = FullRound
            get { return ActionTypeEnum.MoveEquivalent; }
        }

        public override string Description
        {
            get { return "Partial charge";  }
        }

    }
}
