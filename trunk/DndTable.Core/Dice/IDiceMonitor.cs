using System.Collections.Generic;

namespace DndTable.Core.Dice
{
    public interface IDiceMonitor
    {
        List<IDiceRoll> GetAllRolls();
        void Clear();
    }
}
