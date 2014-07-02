using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Armors;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Entities;
using DndTable.Core.Factories;
using DndTable.Core.Items;
using DndTable.Core.Log;
using DndTable.Core.Weapons;

namespace DndTable.Core
{
    public interface IGame
    {
        IBoard GameBoard { get; }

        IDiceMonitor DiceMonitor { get; }
        ILogger Logger { get; }

        bool AddCharacter(ICharacter character, Position position);
        List<ICharacter> GetCharacters();
        bool RemoveCharacter(ICharacter character);

        bool AddMapEntity(Position position, EntityTypeEnum entityType);
        bool RemoveMapEntity(Position position);

        IEncounter StartEncounter();
        IEncounter StartEncounter(List<ICharacter> characters);
        IEncounter CurrentEncounter { get; }
    }
}
