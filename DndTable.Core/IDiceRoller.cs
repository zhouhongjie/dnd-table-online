using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    interface IDiceRoller
    {
        int Roll(int d, int nrOfDice = 1);
    }
}
