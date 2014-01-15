using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    interface IDiceRoller : IDiceMonitor
    {
        int Roll(DiceRollEnum type, int d, int bonus);
        bool Check(DiceRollEnum type, int d, int bonus, int dc);
    }
}
