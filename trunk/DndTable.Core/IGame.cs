﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Factories;
using DndTable.Core.Log;

namespace DndTable.Core
{
    public interface IGame
    {
        IBoard GameBoard { get; }

        IDiceMonitor DiceMonitor { get; }
        ILogger Logger { get; }

        bool AddCharacter(ICharacter character, Position position);
        List<ICharacter> GetCharacters();

        bool AddWall(Position position);
        bool RemoveWall(Position selectedPosition);

        IEncounter StartEncounter();
        IEncounter StartEncounter(List<ICharacter> characters);
        IEncounter CurrentEncounter { get; }

        // Actions
        void EquipWeapon(ICharacter character, IWeapon weapon);
        void EquipArmor(ICharacter character, IArmor armor);
    }
}
