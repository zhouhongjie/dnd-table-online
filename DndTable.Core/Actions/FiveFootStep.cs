using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;

namespace DndTable.Core.Actions
{
    class FiveFootStep : BaseAction, IStraightLineMove
    {
        private ICharacter _character;

        internal FiveFootStep(ICharacter character)
            : base(character)
        {
            _character = character;
        }

        public override ActionTypeEnum Type
        {
            get { return ActionTypeEnum.FiveFootStep; }
        }

        public override string Description
        {
            get { return "5 foot step"; }
        }

        public int MaxRange { get { return 1; } }
        public int MinRange { get { return 1; } }

        public override void Do()
        {
            using (var context = Calculator.CreateActionContext(this))
            {
                _Do();
            }
        }

        private void _Do()
        {
            if (_targetPosition == null)
                throw new InvalidOperationException("Position target expected");
            if (GetTilesDistance(_targetPosition, _character.Position) > 1)
                throw new InvalidOperationException("That was more then 5 foot!");

            if (!Board.MoveEntity(_character.Position, _targetPosition))
                return;

            Register();
        }
    }
}
