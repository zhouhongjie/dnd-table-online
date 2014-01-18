using DndTable.Core.Characters;

namespace DndTable.Core.Actions
{
    public interface IAction
    {
        IAction Target(Position position);
        IAction Target(ICharacter character);

        void Do();
    }
}
