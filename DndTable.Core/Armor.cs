using System;

namespace DndTable.Core
{
    internal class Armor : IArmor
    {
        public ArmorProficiencyEnum Proficiency { get; internal set; }
        public int ArmorBonus { get; internal set; }
        public int MaxDexBonus { get; internal set; }
        public int ArmorCheckPenalty { get; internal set; }
        public int ArcaneSpellFailure { get; internal set; }

        public int AdjustedSpeed(int baseSpeed)
        {
            if (Proficiency == ArmorProficiencyEnum.Light)
                return baseSpeed;

            if (baseSpeed == 20)
                return 15;
            if (baseSpeed == 30)
                return 20;

            throw new NotSupportedException("baseSpeed not supported yet: " + baseSpeed);
        }
    }
}