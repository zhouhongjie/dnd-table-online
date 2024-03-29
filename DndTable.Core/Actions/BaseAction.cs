﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Factories;
using DndTable.Core.Items;
using DndTable.Core.Log;

namespace DndTable.Core.Actions
{
    abstract class BaseAction : IAction
    {
        public ICharacter Executer { get; private set; }

        internal BaseAction(ICharacter executer)
        {
            Executer = executer;
        }

        public abstract void Do();
        public abstract ActionTypeEnum Type { get; }
        public abstract string Description { get; }

        protected Position _targetPosition;
        public IAction Target(Position position)
        {
            _targetPosition = position;
            return this;
        }

        protected ICharacter _targetCharacter;
        public IAction Target(ICharacter character)
        {
            _targetCharacter = character;
            return this;
        }

        protected IDiceRoller DiceRoller { get; private set; }
        protected Encounter Encounter { get; private set; }
        protected Board Board { get; private set; }
        protected AbstractActionFactory ActionFactory { get; private set; }

        public virtual bool RequiresUI { get { return true; } }
        public virtual ActionCategoryEnum Category { get { return ActionCategoryEnum.Other; } }


        internal void Initialize(AbstractActionFactory actionFactory)
        {
            ActionFactory = actionFactory;
            DiceRoller = actionFactory.DiceRoller;
            Encounter = actionFactory.Encounter;
            Board = actionFactory.Board;
        }

        // For unit tests
        internal void Initialize(IDiceRoller diceRoller)
        {
            DiceRoller = diceRoller;
        }

        protected void Register()
        {
            if (Encounter == null)
                return;

            Encounter.RegisterAction(Type);
        }

        protected void HandleAttackOfOpportunity(Calculator.CalculatorActionContext context)
        {
            foreach (var participant in this.Encounter.Participants)
            {
                // No AoO of dead enemies
                if (!participant.CharacterSheet.CanAct())
                    continue;

                // no ally bashing
                if (participant.CharacterSheet.FactionId == Executer.CharacterSheet.FactionId)
                    continue;

                // No AoO when flat-footed
                if (participant.CharacterSheet.Conditions.IsFlatFooted)
                    continue;

                if (ActionHelper.IsInThreatenedArea(Executer, participant))
                {
                    // check participant already did an AoO
                    // TODO: possibly multiple AoO's (combat reflexes)
                    var roundInfo = Encounter.GetRoundInfo(participant);
                    if (roundInfo.AttackOfOpportunityCounter > 0)
                        continue;

                    // Increase counter
                    roundInfo.AttackOfOpportunityCounter++;

                    // handle AttackAction of participant
                    // TODO: requires UI interaction !!!!!! (for the moment auto attack)

                    // Note: AoO is always a MeleeAttack in the proper range (otherwise ThreatenedArea is wrong)
                    context.AoO(participant, Executer);
                    var attackOfOpportunity = ActionFactory.MeleeAttack(participant);
                    attackOfOpportunity.Target(Executer);
                    attackOfOpportunity.Do();
                }
            }
        }

    }
}
