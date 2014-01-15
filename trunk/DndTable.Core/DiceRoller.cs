using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    internal class DiceRoller : IDiceRoller
    {
        class  DiceRoll : IDiceRoll
        {
            public DiceRoll(int d, int roll)
            {
                D = d;
                Roll = roll;
            }

            public int D { get; private set; }
            public int Roll { get; private set; }
        }

        private static readonly Random _randomizer = new Random(DateTime.Now.Millisecond);

        private readonly List<DiceRoll> _rolls = new List<DiceRoll>();

        public int Roll(int d, int nrOfDice = 1)
        {
            var total = 0;
            for (var i=0; i < nrOfDice; i++)
            {
                var roll = new DiceRoll(d, _randomizer.Next(d - 1) + 1);
                _rolls.Insert(0, roll);

                total += roll.Roll;
            }
            return total;
        }

        public IDiceRoll GetLastRoll()
        {
            return _rolls.FirstOrDefault();
        }

        public List<IDiceRoll> GetLastRolls(int max)
        {
            return _rolls.Take(max).Select(roll => roll as IDiceRoll).ToList();
        }
    }

}
