using System.Collections.Generic;

namespace DndTable.Core.Log
{
    public interface ILogger
    {
        List<string> GetAllMessages();
        List<string> GetLast(int count);
        void Clear();
    }
}