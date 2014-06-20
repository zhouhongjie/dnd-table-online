﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Factories;
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


        internal void Initialize(AbstractActionFactory actionFactory)
        {
            ActionFactory = actionFactory;
            DiceRoller = actionFactory.DiceRoller;
            Encounter = actionFactory.Encounter;
            Board = actionFactory.Board;
        }

        protected void Register()
        {
            if (Encounter == null)
                return;

            Encounter.RegisterAction(Type);
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
