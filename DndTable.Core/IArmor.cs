using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    public enum ArmorProficiencyEnum
    {
        Light, Medium, Heavy, Shield
    }

    public interface IArmor
    {
        ArmorProficiencyEnum Proficiency { get; }
        int ArmorBonus { get; }
        int MaxDexBonus { get; }
        int ArmorCheckPenalty { get; }
        int ArcaneSpellFailure { get; }

        int AdjustedSpeed(int baseSpeed);
    }
}
