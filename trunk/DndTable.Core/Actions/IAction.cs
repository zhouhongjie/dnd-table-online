using DndTable.Core.Characters;

namespace DndTable.Core.Actions
{
    public enum ActionTypeEnum
    {
        Standard, MoveEquivalent, FiveFootStep, FullRound
    }

    public interface IAction
    {
        void Do();

        ActionTypeEnum Type { get; }

        string Description { get; }

        bool RequiresUI { get; }
    }
}
