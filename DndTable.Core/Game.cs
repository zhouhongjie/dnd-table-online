﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Entities;
using DndTable.Core.Factories;

namespace DndTable.Core
{
    internal class Game : IGame
    {
        public IBoard GameBoard { get { return _gameBoard; } }
        private Board _gameBoard;

        public IDiceMonitor DiceMonitor { get { return _diceRoller; } }
        private IDiceRoller _diceRoller;

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

        public bool AddWall(Position position)
        {
            return _gameBoard.AddEntity(Factory.CreateWall(), position);
        }

        public bool RemoveWall(Position selectedPosition)
        {
            var entity = _gameBoard.GetEntity(selectedPosition);
            if (entity == null || entity.EntityType != EntityTypeEnum.Wall)
                return false;

            return _gameBoard.RemoveEntity(selectedPosition);
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

        public void EquipWeapon(ICharacter character, IWeapon weapon)
        {
            GetEditableSheet(character).EquipedWeapon = weapon;
        }

        public void EquipArmor(ICharacter character, IArmor armor)
        {
            GetEditableSheet(character).EquipedArmor = armor;
        }

        protected static CharacterSheet GetEditableSheet(ICharacter character)
        {
            var sheet = character.CharacterSheet as CharacterSheet;
            if (sheet == null)
                throw new ArgumentException();
            return sheet;
        }

    }
}
