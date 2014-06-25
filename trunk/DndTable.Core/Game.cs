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
            var entity = _gameBoard.GetEntity(selectedPosition, EntityTypeEnum.Wall);
            if (entity == null)
                return false;

            return _gameBoard.RemoveEntity(entity);
        }

        public bool AddChest(Position position)
        {
            return _gameBoard.AddEntity(Factory.CreateChest(), position);
        }

        public bool RemoveChest(Position selectedPosition)
        {
            var entity = _gameBoard.GetEntity(selectedPosition, EntityTypeEnum.Chest);
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

        public void EquipWeapon(ICharacter character, IWeapon weapon)
        {
            CharacterSheet.GetEditableSheet(character).EquipedWeapon = weapon;
        }

        public void EquipArmor(ICharacter character, IArmor armor)
        {
            CharacterSheet.GetEditableSheet(character).EquipedArmor = armor;
        }

        public void GivePotion(ICharacter character, IPotion potion)
        {
            CharacterSheet.GetEditableSheet(character).Potions.Add(potion);
        }

        public void GiveWeapon(ICharacter character, IWeapon weapon)
        {
            CharacterSheet.GetEditableSheet(character).Weapons.Add(weapon);
        }
    }
}
