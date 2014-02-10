using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.UnityUI
{
    public abstract class BaseActionUI
    {
        public bool IsDone { get; protected set; }
        public bool IsMultiStep { get; protected set; }

        public abstract void Update();
        public abstract void Stop();
    }
}
