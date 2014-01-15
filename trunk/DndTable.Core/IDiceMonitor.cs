using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    public interface IDiceMonitor
    {
        List<IDiceRoll> GetAllRolls();
        void Clear();
    }
}
