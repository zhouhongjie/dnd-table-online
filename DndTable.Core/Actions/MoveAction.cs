﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;

namespace DndTable.Core.Actions
{
    class MoveAction : BaseAction, IMoveAction
    {
        private ICharacter _character;


        internal MoveAction(ICharacter character)
        {
            _character = character;
        }

        public override ActionTypeEnum Type
        {
            get { return ActionTypeEnum.MoveEquivalent; }
        }

        public override string Description { get { return "Move"; } }

        public override void Do()
        {
            //if (_targetPosition == null)
            //    throw new InvalidOperationException("Position target expected");

            Register();

            // TODO: Can move?

            // TODO: Can move to this point?



            // TEMP
            //Board.MoveEntity(_character.Position, _targetPosition);
        }

        public void DoOneStep(Position newLocation)
        {
            // TODO: Can move?

            // TODO: Can move to this point?



            // TEMP
            Board.MoveEntity(_character.Position, newLocation);
        }
    }
}