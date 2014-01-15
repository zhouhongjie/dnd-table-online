namespace DndTable.Core
{
    public enum DiceRollEnum
    {
        Attack, Damage
    }

    public interface IDiceRoll
    {
        DiceRollEnum Type { get; }
        int D { get; }
        int Roll { get; }
        int Bonus { get; }
        int Result { get; }

        bool IsCheck { get; }
        IDiceRollCheck Check { get; }
    }
}