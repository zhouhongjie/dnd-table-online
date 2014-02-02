using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Factories;

namespace DndTable.Core
{
    class Encounter : IEncounter
    {
        private AbstractActionFactory _actionFactory;
        private Board _gameBoard;

        private List<ICharacter> _participants;
        private int _currentIndex = 0;
        private int _currentRound = 0;

        private List<ActionTypeEnum> _actionDoneByCurrentChar = new List<ActionTypeEnum>();


        internal Encounter(Board gameBoard, IDiceRoller diceRoller, List<ICharacter> participants)
        {
            _actionFactory = new AbstractActionFactory(this, gameBoard, diceRoller);
            _gameBoard = gameBoard;

            _participants = DoInitiaticeChecks(diceRoller, participants);
        }

        internal void RegisterAction(ActionTypeEnum actionType)
        {
            _actionDoneByCurrentChar.Add(actionType);
        }

        private static List<ICharacter> DoInitiaticeChecks(IDiceRoller diceRoller, List<ICharacter> participants)
        {
            var initChecks = new List<KeyValuePair<ICharacter, int>>();
            foreach (var participant in participants)
            {
                var initCheck = new KeyValuePair<ICharacter, int>(participant, diceRoller.Roll(participant, DiceRollEnum.InitiativeCheck, 20, participant.CharacterSheet.Initiative));
                initChecks.Add(initCheck);
            }

            var ordered = initChecks.OrderByDescending(c => c.Value);

            var sortedResult = new List<ICharacter>();
            foreach (var kvp in ordered)
            {
                sortedResult.Add(kvp.Key);
            }
            return sortedResult;
        }

        public ICharacter GetCurrentCharacter()
        {
            return _participants[_currentIndex];
        }

        public ICharacter GetNextCharacter()
        {
            if (_currentIndex++ >= _participants.Count - 1)
            {
                _currentRound++;
                _currentIndex = 0;
            }

            _actionDoneByCurrentChar.Clear();

            var currentCharacter = GetCurrentCharacter();

            if (currentCharacter != null)
                _gameBoard.CalculateFieldOfView(currentCharacter.Position);

            return GetCurrentCharacter();
        }

        public int GetRound()
        {
            return _currentRound;
        }

        public List<IAction> GetPossibleActionsForCurrentCharacter()
        {
            var actions = new List<IAction>();

            // 2 partial actions done
            if (_actionDoneByCurrentChar.Count(a => (a == ActionTypeEnum.MoveEquivalent) || (a == ActionTypeEnum.Standard)) >= 2)
                return actions;

            // Check partial actions
            if (!_actionDoneByCurrentChar.Contains(ActionTypeEnum.Standard))
            {
                if (GetCurrentCharacter().CharacterSheet.EquipedWeapon != null)
                {
                    if (GetCurrentCharacter().CharacterSheet.EquipedWeapon.IsRanged)
                        actions.Add(_actionFactory.RangeAttack(GetCurrentCharacter()));
                    else
                        actions.Add(_actionFactory.MeleeAttack(GetCurrentCharacter()));
                }
            }

            // Check MoveEquivalent actions
            if (_actionDoneByCurrentChar.Count(a => a == ActionTypeEnum.MoveEquivalent) < 2)
            {
                actions.Add(_actionFactory.Move(GetCurrentCharacter()));
            }

            return actions;
        }
    }
}
