using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    internal class Character : ICharacter
    {
        public ICharacterSheet CharacterSheet { get; private set; }
        public EntityTypeEnum EntityType { get { return EntityTypeEnum.Character; } }

        public Character(ICharacterSheet sheet)
        {
            CharacterSheet = sheet;
        }
    }
}
