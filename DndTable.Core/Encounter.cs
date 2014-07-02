using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Entities;
using DndTable.Core.Factories;
using DndTable.Core.Log;

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

        public int UseAttackBonus(Calculator.CalculatorPropertyContext context)
        {
            return context.Use(AttackBonus, "RoundInfo.AttackBonus");
        }

        public int UseArmorBonus(Calculator.CalculatorPropertyContext context)
        {
            return context.Use(ArmorBonus, "RoundInfo.ArmorBonus");
        }
    }
        

    class Encounter : IEncounter
    {
        public List<ICharacter> Participants { get; private set; }

        private AbstractActionFactory _actionFactory;

        private int _currentIndex = 0;
        private int _currentRound = 0;

        private List<ActionTypeEnum> _actionDoneByCurrentChar = new List<ActionTypeEnum>();
        private List<IAction> _contextActions = null;

        internal Encounter(Board gameBoard, IDiceRoller diceRoller, List<ICharacter> participants)
        {
            _actionFactory = new AbstractActionFactory(this, gameBoard, diceRoller);

            Participants = DoInitiativeChecks(diceRoller, participants);

            // Init roundInfo
            GetRoundInfo(GetCurrentCharacter()).Reset(GetCurrentCharacter());
        }

        internal void RegisterAction(ActionTypeEnum actionType)
        {
            _actionDoneByCurrentChar.Add(actionType);
            _contextActions = null;
        }

        private static List<ICharacter> DoInitiativeChecks(IDiceRoller diceRoller, List<ICharacter> participants)
        {
            var initChecks = new List<KeyValuePair<ICharacter, int>>();
            foreach (var participant in participants)
            {
                var initCheck = new KeyValuePair<ICharacter, int>(participant, diceRoller.Roll(participant, DiceRollEnum.InitiativeCheck, 20, participant.CharacterSheet.GetCurrentInitiative()));
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
            _contextActions = null;

            GetRoundInfo(current).Reset(current);

            CharacterSheet.GetEditableSheet(current).ApplyEffectsForThisRound();

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

        public bool SetEntityContext(IEntity entity)
        {
            var baseEntity = entity as BaseEntity;
            var contextActions = baseEntity.GetUseActions(GetCurrentCharacter(), _actionFactory);

            if (contextActions == null)
                return false;

            _contextActions = contextActions;
            return true;
        }

        public List<IAction> GetPossibleActionsForCurrentCharacter()
        {
            var actions = new List<IAction>();

            // Temp possibility => example after death due to AoO
            if (!GetCurrentCharacter().CharacterSheet.CanAct())
                return actions;

            // Exceptional action: Check 5-foot-move
            // "If you move no actual distance in a round (commonly because you have swapped your move for one or more equivalent actions), you can take one 5-foot step either before, during, or after the action"
            if (GetRoundInfo(GetCurrentCharacter()).StartPosition == GetCurrentCharacter().Position)
            {
                actions.Add(_actionFactory.FiveFootStep(GetCurrentCharacter()));
            }

            // CHECK ALL DONE
            if (!CanDoMoveEquivalentAction())
                return actions;

            // Check Standard actions
            if (CanDoStandardAction())
            {
                if (GetCurrentCharacter().CharacterSheet.EquipedWeapon != null && !GetCurrentCharacter().CharacterSheet.EquipedWeapon.NeedsReload)
                {
                    if (GetCurrentCharacter().CharacterSheet.EquipedWeapon.IsRanged)
                        actions.Add(_actionFactory.RangeAttack(GetCurrentCharacter()));
                    else
                        actions.Add(_actionFactory.MeleeAttack(GetCurrentCharacter()));
                }
            }

            if (CanDoMoveAction())
            {
                actions.Add(_actionFactory.Move(GetCurrentCharacter()));
            }

            // Check charge (= Full-round action)
            if (CanDoMoveAction() && CanDoFullRoundAction())
            {
                // Cannot charge with ranged weapon
                if (GetCurrentCharacter().CharacterSheet.EquipedWeapon == null || !GetCurrentCharacter().CharacterSheet.EquipedWeapon.IsRanged)
                    actions.Add(_actionFactory.Charge(GetCurrentCharacter()));
            }

            // Check reload action (FullRound || MoveEquivalent => depending on )
            if (GetCurrentCharacter().CharacterSheet.EquipedWeapon != null && GetCurrentCharacter().CharacterSheet.EquipedWeapon.NeedsReload)
            {
                var reloadAction = _actionFactory.Reload(GetCurrentCharacter());
                if (reloadAction.Type == ActionTypeEnum.FullRound && CanDoFullRoundAction())
                    actions.Add(reloadAction);
                else if (reloadAction.Type == ActionTypeEnum.MoveEquivalent && CanDoMoveEquivalentAction())
                    actions.Add(reloadAction);
                else
                {
                    throw new NotSupportedException("Reload action type not supported yet: " + reloadAction.Type);
                }
            }

            // DrinkPotion actions
            if (CanDoStandardAction())
            {
                foreach (var potion in GetCurrentCharacter().CharacterSheet.Potions)
                {
                    actions.Add(_actionFactory.DrinkPotion(GetCurrentCharacter(), potion));
                }
            }

            // CastSpell actions
            // TODO: duration type varies by spell
            if (CanDoStandardAction())
            {
                foreach (var spell in GetCurrentCharacter().CharacterSheet.Spells)
                {
                    actions.Add(_actionFactory.CastSpell(GetCurrentCharacter(), spell));
                }
            }

            // Switch weapon action
            if (CanDoFullRoundAction())
            {
                foreach (var weapon in GetCurrentCharacter().CharacterSheet.Weapons)
                {
                    actions.Add(_actionFactory.SwitchWeapon(GetCurrentCharacter(), weapon));
                }
            }

            // Add entity context actions
            if (_contextActions != null)
            {
                foreach (var contextAction in _contextActions)
                {
                    if (contextAction.Type == ActionTypeEnum.FullRound)
                    {
                        if (CanDoFullRoundAction())
                            actions.Add(contextAction);
                    }
                    else if (contextAction.Type == ActionTypeEnum.MoveEquivalent)
                    {
                        if (CanDoMoveEquivalentAction())
                            actions.Add(contextAction);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }

            return actions;
        }

        private bool CanDoMoveAction()
        {
            return CanDoMoveEquivalentAction() &&
                   (_actionDoneByCurrentChar.Count(a => a == ActionTypeEnum.FiveFootStep) < 1);
        }

        private bool CanDoMoveEquivalentAction()
        {
            return (_actionDoneByCurrentChar.Count(a => a == ActionTypeEnum.MoveEquivalent || a == ActionTypeEnum.Standard) < 2) &&
                   (_actionDoneByCurrentChar.Count(a => a == ActionTypeEnum.FullRound) < 1);
        }

        private bool CanDoStandardAction()
        {
            return CanDoMoveEquivalentAction() &&
                   !_actionDoneByCurrentChar.Contains(ActionTypeEnum.Standard);
        }

        private bool CanDoFullRoundAction()
        {
            return (_actionDoneByCurrentChar.Count(a => a == ActionTypeEnum.MoveEquivalent || a == ActionTypeEnum.Standard) < 1) &&
                   (_actionDoneByCurrentChar.Count(a => a == ActionTypeEnum.FullRound) < 1);
        }

        public RoundInfo GetRoundInfo(ICharacter participant)
        {
            return (participant.CharacterSheet as CharacterSheet).CurrentRoundInfo;
        }
    }
}
