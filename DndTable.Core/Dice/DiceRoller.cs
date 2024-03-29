﻿using System;
using System.Collections.Generic;
using System.Linq;
using DndTable.Core.Characters;
using DndTable.Core.Log;

namespace DndTable.Core.Dice
{
    internal class DiceRoller : IDiceMonitor,  IDiceRoller
    {
        private readonly List<DiceRoll> _rolls = new List<DiceRoll>();
        private readonly IDiceRandomizer _diceRandomizer;

        public DiceRoller(IDiceRandomizer diceRandomizer)
        {
            _diceRandomizer = diceRandomizer;
        }

        public int Roll(ICharacter roller, DiceRollEnum type, int d, int bonus)
        {
            var roll = new DiceRoll(roller, type, 1, d, bonus, _diceRandomizer.Roll(d));
            Logger.Singleton.AddRoll(roll);
            _rolls.Add(roll);
            return roll.Result;
        }

        public int Roll(ICharacter roller, DiceRollEnum type, int nrOfRolls, int d, int bonus)
        {
            int rolls = 0;
            for (var i=0; i < nrOfRolls; i++)
            {
                rolls += _diceRandomizer.Roll(d);
            }

            var roll = new DiceRoll(roller, type, nrOfRolls, d, bonus, rolls);
            Logger.Singleton.AddRoll(roll);
            _rolls.Add(roll);
            return roll.Result;
        }

        public bool Check(ICharacter roller, DiceRollEnum type, int d, int bonus, int dc)
        {
            return RollCheck(roller, type, d, bonus, dc).Success;
        }

        public DiceCheck RollCheck(ICharacter roller, DiceRollEnum type, int d, int bonus, int dc)
        {
            var roll = new DiceCheck(roller, type, d, bonus, _diceRandomizer.Roll(d), dc);
            Logger.Singleton.AddRoll(roll);
            _rolls.Add(roll);
            return roll;
        }

        public AttackRoll RollAttack(ICharacter roller, DiceRollEnum type, int bonus, int dc, int threatRange)
        {
            var roll = new AttackRoll(roller, type, bonus, _diceRandomizer.Roll(20), dc, threatRange);
            Logger.Singleton.AddRoll(roll);
            _rolls.Add(roll);
            return roll;
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
