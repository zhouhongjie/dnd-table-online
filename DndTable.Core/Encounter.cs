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
    class RoundInfo
    {
        public int AttackOfOpportunityCounter;
        public Position StartPosition;
        public int AttackBonus;
        public int ArmorBonus;

        public void Reset(ICharacter character)
        {
            AttackOfOpportunityCounter = 0;
            StartPosition = character.Position;
            AttackBonus = 0;
            ArmorBonus = 0;
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

        internal Encounter(Board gameBoard, IDiceRoller diceRoller, List<ICharacter> participants)
        {
            _actionFactory = new AbstractActionFactory(this, gameBoard, diceRoller);
            _gameBoard = gameBoard;

            Participants = DoInitiaticeChecks(diceRoller, participants);

            // Init roundInfo
            GetRoundInfo(GetCurrentCharacter()).Reset(GetCurrentCharacter());

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

            var current = GetCurrentCharacter();

            _actionDoneByCurrentChar.Clear();
            GetRoundInfo(current).Reset(current);

            // TODO: Handle dying


            // TODO: Handle infinite recursion when all chars are disabled
            if (!current.CharacterSheet.CanAct())
                return GetNextCharacter();

            return current;
        }

        public int GetRound()
        {
            return _currentRound;
        }

        public List<IAction> GetPossibleActionsForCurrentCharacter()
        {
            var actions = new List<IAction>();

            // 2 Standard actions done
            if (_actionDoneByCurrentChar.Count(a => (a == ActionTypeEnum.MoveEquivalent) || (a == ActionTypeEnum.Standard)) >= 2)
                return actions;

            // Check Standard actions
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
            if (_actionDoneByCurrentChar.Count(a => a == ActionTypeEnum.MoveEquivalent || a == ActionTypeEnum.FiveFootStep) < 2)
            {
                actions.Add(_actionFactory.Move(GetCurrentCharacter()));
            }

            // Check 5-foot-move
            // "If you move no actual distance in a round (commonly because you have swapped your move for one or more equivalent actions), you can take one 5-foot step either before, during, or after the action"
            if (GetRoundInfo(GetCurrentCharacter()).StartPosition == GetCurrentCharacter().Position)
            {
                actions.Add(_actionFactory.FiveFootStep(GetCurrentCharacter()));
            }

            // Check charge (= Full-round action)
            if (_actionDoneByCurrentChar.Count == 0)
            {
                actions.Add(_actionFactory.Charge(GetCurrentCharacter()));
            }

            return actions;
        }

        public RoundInfo GetRoundInfo(ICharacter participant)
        {
            return (participant.CharacterSheet as CharacterSheet).CurrentRoundInfo;
        }
    }
}
