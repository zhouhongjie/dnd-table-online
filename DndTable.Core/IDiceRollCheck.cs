using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    public interface IDiceRollCheck
    {
        int DC { get; }
        bool Success { get; }
    }
}
