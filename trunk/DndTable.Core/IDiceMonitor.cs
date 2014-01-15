using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    public interface IDiceMonitor
    {
        IDiceRoll GetLastRoll();
        List<IDiceRoll> GetLastRolls(int max);
    }
}
