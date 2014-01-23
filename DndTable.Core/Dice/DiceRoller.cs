using System;
using System.Collections.Generic;
using System.Linq;
using DndTable.Core.Characters;

namespace DndTable.Core.Dice
{
    internal class DiceRoller : IDiceMonitor,  IDiceRoller
    {
        class  DiceRoll : IDiceRoll
        {
            public static DiceRoll CreateRoll(ICharacter roller, DiceRollEnum type, int d, int bonus, int roll)
            {
                var diceRoll = new DiceRoll()
                               {
                                   Roller = roller,
                                   Type = type,
                                   D = d,
                                   Bonus = bonus,
                                   Roll = roll,
                               };

                diceRoll.Result = diceRoll.Roll + diceRoll.Bonus;
                return diceRoll;
            }

            public static DiceRoll CreateCheck(ICharacter roller, DiceRollEnum type, int d, int bonus, int roll, int dc)
            {
                var diceRoll = CreateRoll(roller, type, d, bonus, roll);
                diceRoll.Check = new DiceRollCheck(diceRoll, dc);
                return diceRoll;
            }

            public ICharacter Roller { get; private set; }
            public DiceRollEnum Type { get; private set; }
            public int D { get; private set; }
            public int Bonus { get; private set; }

            public bool IsCheck
            {
                get { return Check != null; }
            }

            public IDiceRollCheck Check { get; private set; }

            public int Roll { get; private set; }
            public int Result { get; private set; }
        }

        class DiceRollCheck : IDiceRollCheck
        {
            public DiceRollCheck(IDiceRoll roll, int dc)
            {
                DC = dc;

                Success = roll.Result >= DC;
            }

            public int DC { get; private set; }
            public bool Success { get; private set; }
        }


        private readonly List<DiceRoll> _rolls = new List<DiceRoll>();
        private readonly IDiceRandomizer _diceRandomizer;

        public DiceRoller(IDiceRandomizer diceRandomizer)
        {
            _diceRandomizer = diceRandomizer;
        }

        public int Roll(ICharacter roller, DiceRollEnum type, int d, int bonus)
        {
            var roll = DiceRoll.CreateRoll(roller, type, d, bonus, _diceRandomizer.Roll(d));
            _rolls.Add(roll);
            return roll.Result;
        }

        public bool Check(ICharacter roller, DiceRollEnum type, int d, int bonus, int dc)
        {
            var roll = DiceRoll.CreateCheck(roller, type, d, bonus, _diceRandomizer.Roll(d), dc);
            _rolls.Add(roll);
            return roll.Check.Success;
        }

        public List<IDiceRoll> GetAllRolls()
        {
            return _rolls.Select(roll => roll as IDiceRoll).ToList();
        }

        public List<IDiceRoll> GetLastRolls(int nrOfRolls)
        {
            return _rolls.Skip(Math.Max(0, _rolls.Count() - nrOfRolls)).Select(roll => roll as IDiceRoll).ToList();
        }

        public void Clear()
        {
            _rolls.Clear();
        }
    }
}
