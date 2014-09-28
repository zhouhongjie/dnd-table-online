using DndTable.Core.Items;

namespace DndTable.Core.Armors
{
    public enum ArmorProficiencyEnum
    {
        Light, Medium, Heavy, Shield
    }

    public interface IArmor : IItem
    {
        ArmorProficiencyEnum Proficiency { get; }
        int ArmorBonus { get; }
        int MaxDexBonus { get; }
        int ArmorCheckPenalty { get; }
        int ArcaneSpellFailure { get; }

        int AdjustedSpeed(int baseSpeed);
    }
}
