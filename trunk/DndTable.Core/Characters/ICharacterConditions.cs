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
    }
}
