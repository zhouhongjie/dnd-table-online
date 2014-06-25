using DndTable.Core.Entities;
using DndTable.Core.Factories;

namespace DndTable.Core.Characters
{
    public interface ICharacter : IEntity
    {
        ICharacterSheet CharacterSheet { get; }

        CharacterTypeEnum CharacterType { get; }
    }

    public enum CharacterTypeEnum
    {
        Unknown, Hero, Orc, OrcChief
    }
}
