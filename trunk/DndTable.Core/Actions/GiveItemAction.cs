using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Items;

namespace DndTable.Core.Actions
{
    internal class GiveItemAction : BaseAction
    {
        private IItem _item;

        internal GiveItemAction(ICharacter giver, IItem item)
            : base(giver)
        {
            _item = item;
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
            if (_targetCharacter == null)
                throw new InvalidOperationException("Character target expected");

            Register();

            // TODO; check AoO?

            // Remove item
            if (!Executer.RemoveItem(_item))
                throw new InvalidOperationException("Executer doesn't have item");

            _targetCharacter.Give(_item);
        }


        public override ActionTypeEnum Type
        {
            get { return ActionTypeEnum.MoveEquivalent; } // TODO: check
        }

        public override ActionCategoryEnum Category { get { return ActionCategoryEnum.Context; } }

        public override string Description
        {
            get { return "Give " + _item.Description; }
        }

        public override bool RequiresUI
        {
            get { return false; }
        }
    }
}
