using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;

namespace DndTable.Core.Dice
{
    internal class DiceRoll : IDiceRoll
    {
        public DiceRoll(ICharacter roller, DiceRollEnum type, int d, int bonus, int roll)
        {
            Roller = roller;
            Type = type;
            D = d;
            Bonus = bonus;
            Roll = roll;

            Result = Roll + Bonus;
        }

        public ICharacter Roller { get; private set; }
        public DiceRollEnum Type { get; private set; }
        public int D { get; private set; }
        public int Bonus { get; private set; }

        public int Roll { get; private set; }
        public int Result { get; private set; }

        public virtual string Description
        {
            get
            {
                return string.Format("{0}-{1}: {3}(1d{2}) + {4} = {5}",
                                     Roller.CharacterSheet.Name,
                                     Type,
                                     D,
                                     Roll,
                                     Bonus,
                                     Result);
            }
        }
    }
}
