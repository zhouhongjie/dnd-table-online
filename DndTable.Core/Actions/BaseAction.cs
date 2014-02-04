using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Factories;

namespace DndTable.Core.Actions
{
    abstract class BaseAction : IAction
    {
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

        protected static double GetDistance(Position position1, Position position2)
        {
            var dx = position1.X - position2.X;
            var dy = position1.Y - position2.Y;

            return Math.Sqrt(dx * dx + dy * dy);
        }

        protected static int GetTilesDistance(Position position1, Position position2)
        {
            return (int)Math.Floor(GetDistance(position1, position2));
        }
    }
}
