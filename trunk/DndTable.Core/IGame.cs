using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Armors;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
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

        bool AddWall(Position position);
        bool RemoveWall(Position selectedPosition);

        bool AddChest(Position position);
        bool RemoveChest(Position selectedPosition);

        IEncounter StartEncounter();
        IEncounter StartEncounter(List<ICharacter> characters);
        IEncounter CurrentEncounter { get; }

        // Actions
        void EquipWeapon(ICharacter character, IWeapon weapon);
        void EquipArmor(ICharacter character, IArmor armor);
        void GivePotion(ICharacter character, IPotion potion);
        void GiveWeapon(ICharacter character, IWeapon weapon);
    }
}
