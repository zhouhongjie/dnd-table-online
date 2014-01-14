using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    public interface IGame
    {
        IBoard GameBoard { get; }


        bool AddCharacter(ICharacter character, int x, int y);
        List<ICharacter> GetCharacters();


        void MeleeAttack(ICharacter attacker, ICharacter target);
    }
}
