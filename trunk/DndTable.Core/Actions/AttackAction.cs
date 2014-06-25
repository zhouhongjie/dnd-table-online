using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Log;
using DndTable.Core.Weapons;

namespace DndTable.Core.Actions
{
    class AttackAction : BaseAction, IAttackAction
    {
        internal AttackAction(ICharacter attacker)
            : base(attacker)
        {}

        public override ActionTypeEnum Type
        {
            get { return ActionTypeEnum.Standard; }
        }

        public override ActionCategoryEnum Category
        {
            get { return ActionCategoryEnum.Combat; }
        }

        public override string Description
        {
            get
            {
                if (Executer.CharacterSheet.EquipedWeapon == null)
                    return "Unarmed attack NOT SUPPORTED YET";
                if (Executer.CharacterSheet.EquipedWeapon.IsRanged)
                    return "Ranged attack";

                return "Melee attack";
            }
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

            // Has weapon?
            if ((Executer.CharacterSheet.EquipedWeapon == null))
                throw new InvalidOperationException("attacker has no equiped weapon");

            // Needs reload?
            if (Executer.CharacterSheet.EquipedWeapon.NeedsReload)
                throw new InvalidOperationException("attacker's equiped weapon needs reload");

            // Check max range
            var rangeRounded = MathHelper.GetTilesDistance(Executer.Position, _targetCharacter.Position);
            if (rangeRounded > MaxRange)
                throw new InvalidOperationException("Out of range: should have been checked before calling this method");

            Register();


            // Check counter AttackOfOpportunity
            if (ProvokesAttackOfOpportunity())
            {
                HandleAttackOfOpportunity(context);

                if (!Executer.CharacterSheet.CanAct())
                    return;
            }

            // Use the weapon (Launch ammo is required)
            var weapon = (Executer.CharacterSheet.EquipedWeapon as Weapon);
            weapon.Use();

            // Check hit
            var check = DiceRoller.RollAttack(
                Executer, 
                DiceRollEnum.Attack,
                Executer.CharacterSheet.GetCurrentAttackBonus((int)rangeRounded * 5, IsFlanking()),  // Convert tiles to feet
                _targetCharacter.CharacterSheet.GetCurrentArmorClass(),
                20 - Executer.CharacterSheet.EquipedWeapon.CriticalRange);

            if (!check.Success)
                return;

            // Critical hit
            bool isCritical = false;
            if (check.IsThreat)
            {
                isCritical = DiceRoller.Check(
                    Executer, 
                    DiceRollEnum.CriticalAttack, 
                    20,
                    Executer.CharacterSheet.GetCurrentAttackBonus((int)rangeRounded * 5, IsFlanking()),
                    _targetCharacter.CharacterSheet.GetCurrentArmorClass());
            }


            // Do damage
            var nrOfDamageRolls = isCritical ? Executer.CharacterSheet.EquipedWeapon.CriticalMultiplier : 1;
            for (var i = 0; i < nrOfDamageRolls; i++)
            {
                var damage = DiceRoller.Roll(
                    Executer, 
                    DiceRollEnum.Damage, 
                    Executer.CharacterSheet.EquipedWeapon.DamageD, 
                    Executer.CharacterSheet.GetCurrentDamageBonus());

                if (damage < 1)
                    damage = 1;

                CharacterSheet.GetEditableSheet(_targetCharacter).HitPoints -= damage;
            }
        }

        private bool IsFlanking()
        {
            return ActionHelper.IsFlanking(Executer, _targetCharacter, this.Encounter.Participants);
        }

        private bool ProvokesAttackOfOpportunity()
        {
            return Executer.CharacterSheet.EquipedWeapon.IsRanged;
        }

        public int MaxRange
        {
            get 
            {
                if (Executer.CharacterSheet.EquipedWeapon != null && Executer.CharacterSheet.EquipedWeapon.IsRanged)
                {
                    // TODO: implement in weapon stats?
                    // Thrown weapons: range increment x5
                    // Projectile weapons: range increment x10

                    // x / 5 (feet to tiles) 

                    return (int) Math.Floor(Executer.CharacterSheet.EquipedWeapon.RangeIncrement*2.0);
                }


                // TODO: reach weapons
                // Implement in weapon stats?
                return 1;
            }
        }

        public int MinRange
        {
            get
            {
                // TODO: reach weapons
                // Implement in weapon stats?
                return 0;
            }
        }
    }
}
