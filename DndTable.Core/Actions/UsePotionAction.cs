using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Items;

namespace DndTable.Core.Actions
{
    internal class UsePotionAction : BaseAction
    {
        internal UsePotionAction(ICharacter attacker, IPotion potion)
            : base(attacker)
        {
            _targetPotion = potion;
        }

        private IPotion _targetPotion;

        public override void Do()
        {
            using (var context = Calculator.CreateActionContext(this))
            {
                _Do(context);
            }
        }

        public override ActionTypeEnum Type
        {
            get { return ActionTypeEnum.Standard; }
        }

        public override string Description
        {
            get { return "Drink potion: " + _targetPotion.Description; }
        }

        public override bool RequiresUI
        {
            get { return false; }
        }

        private void _Do(Calculator.CalculatorActionContext context)
        {
            if (_targetPotion == null)
                throw new InvalidOperationException("Potion target expected");

            if (!CharacterSheet.GetEditableSheet(Executer).Potions.Contains(_targetPotion))
                throw new InvalidOperationException("Potion is not in the possession of the executer");

            Register();

            HandleAttackOfOpportunity(context);
            if (!Executer.CharacterSheet.CanAct())
                return;

            if (!(_targetPotion as BasePotion).Use(Executer, DiceRoller))
                return;

            // Remove potion from inventory
            CharacterSheet.GetEditableSheet(Executer).Potions.Remove(_targetPotion);
        }
    }
}
