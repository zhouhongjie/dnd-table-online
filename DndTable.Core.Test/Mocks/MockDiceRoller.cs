﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using Moq;

namespace DndTable.Core.Test.Mocks
{
    internal class MockDiceRoller : IDiceRoller
    {
        #region IDiceRoller
        public int Roll(ICharacter roller, DiceRollEnum type, int d, int bonus)
        {
            return MockRoll;
        }

        public bool Check(ICharacter roller, DiceRollEnum type, int d, int bonus, int dc)
        {
            return MockCheck;
        }

        public DiceCheck RollCheck(ICharacter roller, DiceRollEnum type, int d, int bonus, int dc)
        {
            throw new NotImplementedException();
        }

        public AttackRoll RollAttack(ICharacter roller, DiceRollEnum type, int bonus, int dc, int threatRange)
        {
            return MockAttackRoll;
        }

        #endregion

        #region IDiceMonitor
        public List<IDiceRoll> GetAllRolls()
        {
            throw new NotImplementedException();
        }

        public List<IDiceRoll> GetLastRolls(int nrOfRolls)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }
        #endregion


        internal int MockRoll { get; set; }
        internal bool MockCheck { get; set; }
        internal AttackRoll MockAttackRoll { get; set; }
    }
}