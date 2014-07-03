using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;

namespace DndTable.Core.Dice
{
    internal class DiceCheck : DiceRoll
    {
        public DiceCheck(ICharacter roller, DiceRollEnum type, int d, int bonus, int roll, int dc)
            : base(roller, type, 1, d, bonus, roll)
        {
            DC = dc;
            Success = Result >= DC;
        }

        public int DC { get; private set; }

        public bool Success { get; protected set; }

        public override string Description
        {
            get
            {
                return string.Format("{0}-{1}: {3}(1d{2}) + {4} = {5}; DC = {6} => {7}",
                                     Roller.CharacterSheet.Name,
                                     Type,
                                     D,
                                     Roll,
                                     Bonus,
                                     Result,
                                     DC,
                                     Success ? "Success" : "fail");
            }
        }

    }
}
