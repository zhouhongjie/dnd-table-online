using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    internal class DiceRoller : IDiceRoller
    {
        private static Random _randomizer = new Random(DateTime.Now.Millisecond);

        public int Roll(int d, int nrOfDice = 1)
        {
            var total = 0;
            for (var i=0; i < nrOfDice; i++)
            {
                total += _randomizer.Next(d - 1) + 1;
            }
            return total;
        }
    }
}
