﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Factories;

namespace DndTable.Core
{
    class RoundInfo
    {
        public int AttackOfOpportunityCounter;

        public void Reset()
        {
            AttackOfOpportunityCounter = 0;
        }
    }
        

    class Encounter : IEncounter
    {
        public List<ICharacter> Participants { get; private set; }

        private AbstractActionFactory _actionFactory;
        private Board _gameBoard;

        private int _currentIndex = 0;
        private int _currentRound = 0;

        private List<ActionTypeEnum> _actionDoneByCurrentChar = new List<ActionTypeEnum>();
        private Dictionary<ICharacter, RoundInfo> _roundInfo = new Dictionary<ICharacter, RoundInfo>();


        internal Encounter(Board gameBoard, IDiceRoller diceRoller, List<ICharacter> participants)
        {
            _actionFactory = new AbstractActionFactory(this, gameBoard, diceRoller);
            _gameBoard = gameBoard;

            Participants = DoInitiaticeChecks(diceRoller, participants);

            //_gameBoard.CalculateFieldOfView(GetCurrentCharacter().Position);
        }

        internal void RegisterAction(ActionTypeEnum actionType)
        {
            _actionDoneByCurrentChar.Add(actionType);


            //// FoV
            //var currentCharacter = GetCurrentCharacter();
            //if (currentCharacter != null)
            //    _gameBoard.CalculateFieldOfView(currentCharacter.Position);

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
            return Participants[_currentIndex];
        }

        public ICharacter GetNextCharacter()
        {
            if (_currentIndex++ >= Participants.Count - 1)
            {
                _currentRound++;
                _currentIndex = 0;
            }

            _actionDoneByCurrentChar.Clear();
            GetRoundInfo(GetCurrentCharacter()).Reset();

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

        public RoundInfo GetRoundInfo(ICharacter participant)
        {
            RoundInfo roundInfo;
            if (!_roundInfo.TryGetValue(participant, out roundInfo))
            {
                roundInfo = new RoundInfo();
                _roundInfo.Add(participant, roundInfo);
            }
            return roundInfo;
        }
    }
}