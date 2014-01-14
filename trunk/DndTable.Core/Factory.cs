using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    public static class Factory
    {
        public static IGame CreateGame(int maxX, int maxY)
        {
            var board = new Board(maxX, maxY);
            var diceRoller = new DiceRoller();
            return new Game(board, diceRoller);
        }

        public static ICharacter CreateCharacter()
        {
            var sheet = new CharacterSheet();
            return new Character(sheet);
        }
    }
}
