namespace DndTable.Core.Dice
{
    interface IDiceRoller : IDiceMonitor
    {
        int Roll(DiceRollEnum type, int d, int bonus);
        bool Check(DiceRollEnum type, int d, int bonus, int dc);
    }
}
