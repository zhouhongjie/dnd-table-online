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
            return new Game(board);
        }

        public static ICharacter CreateCharacter()
        {
            var sheet = new CharacterSheet();
            return new Character(sheet);
        }
    }
}
