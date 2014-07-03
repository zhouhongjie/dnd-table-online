using DndTable.Core.Characters;

namespace DndTable.Core.Dice
{
    interface IDiceRoller : IDiceMonitor
    {
        int Roll(ICharacter roller, DiceRollEnum type, int d, int bonus);
        int Roll(ICharacter roller, DiceRollEnum type, int nrOfRolls, int d, int bonus);
        bool Check(ICharacter roller, DiceRollEnum type, int d, int bonus, int dc);
        DiceCheck RollCheck(ICharacter roller, DiceRollEnum type, int d, int bonus, int dc);
        AttackRoll RollAttack(ICharacter roller, DiceRollEnum type, int bonus, int dc, int threatRange);
    }
}
