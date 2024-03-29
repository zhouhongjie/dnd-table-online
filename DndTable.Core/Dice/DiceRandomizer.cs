﻿using System;

namespace DndTable.Core.Dice
{
    class DiceRandomizer : IDiceRandomizer
    {
        private static readonly Random _randomizer = new Random(DateTime.Now.Millisecond);

        public int Roll(int d)
        {
            return _randomizer.Next(d) + 1;
        }
    }
}