using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;

namespace DndTable.Core.Dice
{
    internal class AttackRoll : DiceCheck
    {
        public AttackRoll(ICharacter roller, DiceRollEnum type, int bonus, int roll, int dc, int threatRange) 
            : base(roller, type, 20, bonus, roll, dc)
        {
            // Automatic failure
            if (Roll == 1)
            {
                Success = false;
            }

            // Automatic success
            if (Roll == 20)
            {
                Success = true;
            }

            // Critical hit
            IsThreat = Roll >= threatRange;
        }

        public bool IsThreat { get; private set; }
    }
}
