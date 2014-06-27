using DndTable.Core.Characters;

namespace DndTable.Core.Dice
{
    public enum DiceRollEnum
    {
        Attack, CriticalAttack, Damage, InitiativeCheck, PotionEffect, Loot, SpellEffect
    }

    public interface IDiceRoll
    {
        string Description { get; }
    }
}