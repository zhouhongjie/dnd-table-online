using DndTable.Core.Characters;

namespace DndTable.Core.Dice
{
    interface IDiceRoller : IDiceMonitor
    {
        int Roll(ICharacter roller, DiceRollEnum type, int d, int bonus);
        bool Check(ICharacter roller, DiceRollEnum type, int d, int bonus, int dc);
    }
}
