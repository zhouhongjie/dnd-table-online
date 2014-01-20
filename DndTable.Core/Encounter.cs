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
        private IDiceRoller _diceRoller;

        private List<ICharacter> _participants;
        private int _currentIndex = 0;
        private int _currentRound = 0;

        internal Encounter(AbstractActionFactory actionFactory, IDiceRoller diceRoller, List<ICharacter> participants)
        {
            _actionFactory = actionFactory;
            _diceRoller = diceRoller;

            _participants = DoInitiaticeChecks(diceRoller, participants);
        }

        private static List<ICharacter> DoInitiaticeChecks(IDiceRoller diceRoller, List<ICharacter> participants)
        {
            var initChecks = new List<KeyValuePair<ICharacter, int>>();
            foreach (var participant in participants)
            {
                var initCheck = new KeyValuePair<ICharacter, int>(participant, diceRoller.Roll(DiceRollEnum.InitiativeCheck, 20, participant.CharacterSheet.Initiative));
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

            return GetCurrentCharacter();
        }

        public int GetRound()
        {
            return _currentRound;
        }

        List<IAction> IEncounter.GetPossibleActionsForCurrentCharacter()
        {
            // TODO: check possibilities

            var actions = new List<IAction>();
            if (GetCurrentCharacter().CharacterSheet.EquipedWeapon != null)
            {
                if (GetCurrentCharacter().CharacterSheet.EquipedWeapon.IsRanged)
                    actions.Add(_actionFactory.RangeAttack(GetCurrentCharacter()));
                else
                    actions.Add(_actionFactory.MeleeAttack(GetCurrentCharacter()));
            }
            actions.Add(_actionFactory.Move(GetCurrentCharacter()));
            return actions;
        }
    }
}
