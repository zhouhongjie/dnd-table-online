using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;

namespace DndTable.Core.Actions
{
    class MoveAction : BaseAction
    {
        private Board _board;
        private ICharacter _character;


        internal MoveAction(Board board, ICharacter character)
        {
            _board = board;
            _character = character;
        }

        public override void Do()
        {
            if (_targetPosition == null)
                throw new InvalidOperationException("Position target expected");

            // Can move

            // Can move to this point



            // TEMP
            _board.MoveEntity(_character.Position, _targetPosition);
        }
    }
}
