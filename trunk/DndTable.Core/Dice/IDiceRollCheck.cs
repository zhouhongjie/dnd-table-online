namespace DndTable.Core.Dice
{
    public interface IDiceRollCheck
    {
        int DC { get; }
        bool Success { get; }
    }
}
