using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Dice;
using Moq;

namespace DndTable.Core.Test.Helpers
{
    internal static class DiceHelper
    {
        public static DiceRoller CreateNullDiceRoller()
        {
            var diceRandomizer = new Mock<IDiceRandomizer>();

            // always return 0 = no random
            diceRandomizer.Setup(dr => dr.Roll(It.IsAny<int>())).Returns(0);

            var diceRoller = new DiceRoller(diceRandomizer.Object);
            return diceRoller;
        }

    }
}
