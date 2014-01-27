namespace DndTable.Core.Dice
{
    public interface IDiceCheck : IDiceRoll
    {
        int DC { get; }
        bool Success { get; }
    }
}
