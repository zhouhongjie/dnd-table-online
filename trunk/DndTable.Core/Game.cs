using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    internal class Game : IGame
    {
        public IBoard GameBoard { get { return _gameBoard; } }
        private Board _gameBoard;

        private List<ICharacter> _characters = new List<ICharacter>();

        public Game(Board board)
        {
            _gameBoard = board;
        }

        public bool AddCharacter(ICharacter character, int x, int y)
        {
            if (_characters.Contains(character))
                return false;

            if (!_gameBoard.AddEntity(character, x, y))
                return false;

            _characters.Add(character);
            return true;
        }

        public List<ICharacter> GetCharacters()
        {
            return _characters;
        }
    }
}
