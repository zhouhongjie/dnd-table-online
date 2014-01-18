using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Factories;

namespace DndTable.Core
{
    public interface IGame
    {
        IBoard GameBoard { get; }

        IDiceMonitor DiceMonitor { get; }

        bool AddCharacter(ICharacter character, Position position);
        List<ICharacter> GetCharacters();

        IEncounter StartEncounter(List<ICharacter> characters);

        AbstractActionFactory ActionFactory { get; }

        // Actions
        void EquipWeapon(ICharacter character, IWeapon weapon);
        void EquipArmor(ICharacter character, IArmor armor);

    }
}
