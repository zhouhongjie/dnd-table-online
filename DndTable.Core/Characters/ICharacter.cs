using DndTable.Core.Entities;

namespace DndTable.Core.Characters
{
    public interface ICharacter : IEntity
    {
        ICharacterSheet CharacterSheet { get; }


    }
}
