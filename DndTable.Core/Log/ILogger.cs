using System.Collections.Generic;

namespace DndTable.Core.Log
{
    public interface ILogger
    {
        List<string> GetAllMessages();
        void Clear();
    }
}