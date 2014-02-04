using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;

namespace DndTable.Core.Actions
{
    class MoveAction : BaseAction, IMoveAction
    {
        private ICharacter _character;
        private int _maxNrOfSteps;
        private int _nrOfStepsCounter;
        private bool _isDone;

        internal MoveAction(ICharacter character)
        {
            _character = character;
            _maxNrOfSteps = character.CharacterSheet.Speed/5;
        }

        public override ActionTypeEnum Type
        {
            get { return ActionTypeEnum.MoveEquivalent; }
        }

        public override string Description { get { return "Move"; } }

        public override void Do()
        {
            if (_isDone)
                throw new InvalidOperationException("Action was already stopped");

            _isDone = true;
            Register();
        }

        public bool DoOneStep(Position newLocation)
        {
            if (_isDone)
                return false;
            if (GetTilesDistance(newLocation, _character.Position) > 1)
                return false;
            if (_nrOfStepsCounter >= _maxNrOfSteps)
                return false;

            // TODO: Can move? (paralysed, disabled, ...)

            // Can move to this point? (wall, other player, ...)
            if (!Board.MoveEntity(_character.Position, newLocation))
                return false;


            _nrOfStepsCounter++;

            return true;
        }
    }
}
