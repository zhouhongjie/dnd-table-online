using DndTable.Core.Characters;

namespace DndTable.Core.Dice
{
    public enum DiceRollEnum
    {
        Attack, CriticalAttack, Damage, InitiativeCheck
    }

    public interface IDiceRoll
    {
        string Description();
    }
}