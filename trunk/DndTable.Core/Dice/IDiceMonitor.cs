using System.Collections.Generic;

namespace DndTable.Core.Dice
{
    public interface IDiceMonitor
    {
        List<IDiceRoll> GetAllRolls();
        List<IDiceRoll> GetLastRolls(int nrOfRolls);
        void Clear();
    }
}
