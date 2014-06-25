using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Weapons;

namespace DndTable.Core.Actions
{
    internal class SwitchWeaponAction : BaseAction
    {
        private IWeapon _targetWeapon;

        internal SwitchWeaponAction(ICharacter executer, IWeapon newWeapon)
            : base(executer)
        {
            _targetWeapon = newWeapon;
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
            if (Executer.CharacterSheet.EquipedWeapon == null)
                throw new InvalidOperationException("No weapon equiped");

            if (_targetWeapon == null)
                throw new InvalidOperationException("Weapon target expected");

            if (!CharacterSheet.GetEditableSheet(Executer).Weapons.Contains(_targetWeapon))
                throw new InvalidOperationException("Weapon is not in the possession of the executer");

            Register();

            HandleAttackOfOpportunity(context);
            if (!Executer.CharacterSheet.CanAct())
                return;

            var sheet = CharacterSheet.GetEditableSheet(Executer);
            sheet.Weapons.Remove(_targetWeapon);
            sheet.Weapons.Add(sheet.EquipedWeapon);
            sheet.EquipedWeapon = _targetWeapon;
        }

        public override ActionTypeEnum Type
        {
            get { return ActionTypeEnum.FullRound; }
        }

        public override string Description
        {
            get { return "Equip weapon: " + _targetWeapon.Description; }
        }

        public override bool RequiresUI
        {
            get { return false; }
        }
    }
}
