using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Dice;

namespace DndTable.Core.Test.Mocks
{
    internal class MockDiceRoller : IDiceRoller
    {
        #region IDiceRoller
        public int Roll(DiceRollEnum type, int d, int bonus)
        {
            return MockRoll;
        }

        public bool Check(DiceRollEnum type, int d, int bonus, int dc)
        {
            return MockCheck;
        }
        #endregion

        #region IDiceMonitor
        public List<IDiceRoll> GetAllRolls()
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
    }
}
