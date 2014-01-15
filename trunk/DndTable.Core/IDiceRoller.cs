using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    interface IDiceRoller : IDiceMonitor
    {
        int Roll(int d, int nrOfDice = 1);
    }
}
