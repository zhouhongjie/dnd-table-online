using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Entities;

namespace DndTable.Core.Actions
{
    internal class UseDoorAction : BaseAction
    {
       private Door _door;

       internal UseDoorAction(ICharacter executer, Door door)
            : base(executer)
        {
            _door = door;
        }

        public override void Do()
        {
            using (var context = Calculator.CreateActionContext(this))
            {
                _Do(context);
            }
        }

        private void _Do(Calculator.CalculatorActionContext context)
        {
            Register();

            _door.IsOpen = !_door.IsOpen;
        }

        public override ActionTypeEnum Type
        {
            get { return ActionTypeEnum.MoveEquivalent; }
        }

        public override string Description
        {
            get { return _door.IsOpen ? "Close door" : "Open door"; }
        }

        public override bool RequiresUI
        {
            get { return false; }
        }    
    }
}
