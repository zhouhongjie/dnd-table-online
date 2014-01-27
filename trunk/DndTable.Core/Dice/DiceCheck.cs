using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;

namespace DndTable.Core.Dice
{
    internal class DiceCheck : DiceRoll, IDiceCheck
    {
        public DiceCheck(ICharacter roller, DiceRollEnum type, int d, int bonus, int roll, int dc)
            : base(roller, type, d, bonus, roll)
        {
            DC = dc;
            Success = Result >= DC;
        }

        public int DC { get; private set; }

        public bool Success { get; protected set; }
    }
}
