using DndTable.Core.Characters;

namespace DndTable.Core.Dice
{
    public enum DiceRollEnum
    {
        Attack, CriticalAttack, Damage, InitiativeCheck
    }

    public interface IDiceRoll
    {
        ICharacter Roller { get;  }
        DiceRollEnum Type { get; }
        int D { get; }
        int Roll { get; }
        int Bonus { get; }
        int Result { get; }
    }
}