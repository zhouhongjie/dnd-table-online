using DndTable.Core.Characters;

namespace DndTable.Core.Dice
{
    public enum DiceRollEnum
    {
        Attack, CriticalAttack, Damage, InitiativeCheck, Loot, MagicEffect, ResistEffect, Duration,
        Concentration, SneakAttack, Hitpoints
    }

    public interface IDiceRoll
    {
        string Description { get; }
    }
}