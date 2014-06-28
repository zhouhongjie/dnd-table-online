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
    internal class Game : IGame
    {
        public IBoard GameBoard { get { return _gameBoard; } }
        private Board _gameBoard;

        public IDiceMonitor DiceMonitor { get { return _diceRoller; } }
        private IDiceRoller _diceRoller;

        public ILogger Logger { get { return Log.Logger.Singleton; } }

        private List<ICharacter> _characters = new List<ICharacter>();

        public Game(Board board, IDiceRoller diceRoller)
        {
            _gameBoard = board;
            _diceRoller = diceRoller;
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

        public bool RemoveCharacter(ICharacter character)
        {
            if (!_characters.Contains(character))
                return false;

            if (CurrentEncounter.Participants.Contains(character))
                throw new NotSupportedException("Character cannot be removed: part of current encounter");

            if (!_gameBoard.RemoveEntity(character))
                return false;

            _characters.Remove(character);

            return true;
        }

        public bool AddWall(Position position)
        {
            return _gameBoard.AddEntity(Factory.CreateWall(), position);
        }

        public bool RemoveWall(Position selectedPosition)
        {
            return _RemoveEntity(selectedPosition, EntityTypeEnum.Wall);
        }

        public bool AddChest(Position position)
        {
            return _gameBoard.AddEntity(Factory.CreateChest(), position);
        }

        public bool RemoveChest(Position selectedPosition)
        {
            return _RemoveEntity(selectedPosition, EntityTypeEnum.Chest);
        }

        public bool AddDoor(Position position)
        {
            return _gameBoard.AddEntity(Factory.CreateDoor(), position);
        }

        public bool RemoveDoor(Position selectedPosition)
        {
            return _RemoveEntity(selectedPosition, EntityTypeEnum.Door);
        }

        private bool _RemoveEntity(Position selectedPosition, EntityTypeEnum entityType)
        {
            var entity = _gameBoard.GetEntity(selectedPosition, entityType);
            if (entity == null)
                return false;

            return _gameBoard.RemoveEntity(entity);
        }

        public List<ICharacter> GetCharacters()
        {
            return _characters;
        }

        public IEncounter StartEncounter()
        {
            return StartEncounter(_characters);
        }

        public IEncounter StartEncounter(List<ICharacter> characters)
        {
            // Check characters 

            CurrentEncounter = new Encounter(_gameBoard, _diceRoller, characters);
            return CurrentEncounter;
        }

        public IEncounter CurrentEncounter { get; private set; }
    }
}
