using System;
using DndTable.Core.Dice;

namespace DndTable.Core.Characters
{
    internal class Character : BaseEntity, ICharacter
    {
        public ICharacterSheet CharacterSheet { get; private set; }
        public override EntityTypeEnum EntityType { get { return EntityTypeEnum.Character; } }

        public Character(ICharacterSheet sheet)
        {
            CharacterSheet = sheet;
        }
    }
}
