﻿using System;
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

        public ClassBuilder ClassBuilder { get; private set; }

        private List<ICharacter> _characters = new List<ICharacter>();

        public Game(Board board, IDiceRoller diceRoller)
        {
            _gameBoard = board;
            _diceRoller = diceRoller;
            ClassBuilder = new ClassBuilder(_diceRoller);
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
            // Only PC's are added to this list, not NPCs (for now)
            //if (!_characters.Contains(character))
            //    return false;

            if (CurrentEncounter.Participants.Contains(character))
                throw new NotSupportedException("Character cannot be removed: part of current encounter");

            if (!_gameBoard.RemoveEntity(character))
                return false;

            _characters.Remove(character);

            return true;
        }

        public bool AddMapEntity(Position position, EntityTypeEnum entityType)
        {
            return _gameBoard.AddEntity(Factory.CreateEntity(entityType), position);
        }

        public bool RemoveMapEntity(Position position)
        {
            var entities = _gameBoard.GetEntities(position);

            foreach (var entity in entities)
            {
                _gameBoard.RemoveEntity(entity);
            }

            return entities.Count > 0;
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

            CurrentEncounter = new Encounter(_gameBoard, _diceRoller, characters, new List<ICharacter>());
            return CurrentEncounter;
        }

        public IEncounter StartEncounter(List<ICharacter> awareCharacters, List<ICharacter> unawareCharacters)
        {
            // Check characters 

            CurrentEncounter = new Encounter(_gameBoard, _diceRoller, awareCharacters, unawareCharacters);
            return CurrentEncounter;
        }

        public IEncounter CurrentEncounter { get; private set; }
    }
}
