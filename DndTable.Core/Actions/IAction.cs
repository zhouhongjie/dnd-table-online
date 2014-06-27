using DndTable.Core.Characters;

namespace DndTable.Core.Actions
{
    public enum ActionTypeEnum
    {
        Standard, MoveEquivalent, FiveFootStep, FullRound
    }

    public enum ActionCategoryEnum
    {
        Other, Combat, Move, Spell
    }

    public interface IAction
    {
        void Do();

        ActionTypeEnum Type { get; }
        ActionCategoryEnum Category { get; }

        string Description { get; }

        bool RequiresUI { get; }
    }
}
