using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    public interface IGame
    {
        IBoard GameBoard { get; }


        bool AddCharacter(ICharacter character, Position position);
        List<ICharacter> GetCharacters();


        // Actions
        void MeleeAttack(ICharacter attacker, ICharacter target);
        void Move(ICharacter character, Position to);
        void EquipWeapon(ICharacter character, IWeapon weapon);
    }
}
