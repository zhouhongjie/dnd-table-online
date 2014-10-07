using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core.Characters
{
    public interface ICharacterConditions
    {
        bool IsSleeping { get; }
        bool IsHelpless { get; }
        bool IsFlatFooted { get; }
        bool IsParalyzed { get; }

        bool CanDoOnlyPartialActions { get; }
    }
}
