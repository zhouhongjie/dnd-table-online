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

        private IDiceRoller _diceRoller;

        private List<ICharacter> _characters = new List<ICharacter>();

        public Game(Board board, IDiceRoller diceRoller)
        {
            _gameBoard = board;
        }

        public bool AddCharacter(ICharacter character, Position position)
        {
            if (_characters.Contains(character))
                return false;

            if (!_gameBoard.AddEntity(character, position))
                return false;

            _characters.Add(character);
            return true;
        }

        public List<ICharacter> GetCharacters()
        {
            return _characters;
        }

        public void MeleeAttack(ICharacter attacker, ICharacter target)
        {
            // Can reach


            // Check hit
            var attackRoll = _diceRoller.Roll(20) + attacker.CharacterSheet.MeleeAttackBonus;

            // Check crit failure

            if (attackRoll < target.CharacterSheet.ArmourClass)
                return;

            // Check crit

            // Do damage

        }

        public void Move(ICharacter character, Position to)
        {
            // Can move

            // Can move to this point

            

            // TEMP
            _gameBoard.MoveEntity(character.Position, to);
        }
    }
}
