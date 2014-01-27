using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;

namespace DndTable.Core.Actions
{
    class MeleeAttackAction : BaseAction, IAttackAction
    {
        private ICharacter _attacker;

        internal MeleeAttackAction(ICharacter attacker)
        {
            _attacker = attacker;
        }

        public override ActionTypeEnum Type
        {
            get { return ActionTypeEnum.Standard; }
        }

        public override void Do()
        {
            if (_targetCharacter == null)
                throw new InvalidOperationException("Character target expected");

            // Has melee weapon?
            if ((_attacker.CharacterSheet.EquipedWeapon == null) || _attacker.CharacterSheet.EquipedWeapon.IsRanged)
                throw new ArgumentException("attacker has no melee equiped weapon");

            // Check max range
            var rangeRounded = Math.Floor(GetDistance(_attacker.Position, _targetCharacter.Position));
            if (rangeRounded > MaxRange)
                throw new InvalidOperationException("Out of melee range: should have been checked before calling this method");

            Register();


            // Check hit
            var check = DiceRoller.RollAttack(
                _attacker, 
                DiceRollEnum.Attack, 
                _attacker.CharacterSheet.MeleeAttackBonus, 
                _targetCharacter.CharacterSheet.ArmorClass,
                20 - _targetCharacter.CharacterSheet.EquipedWeapon.CriticalRange);

            if (!check.Success)
                return;

            // Critical hit
            bool isCritical = false;
            if (check.IsThreat)
            {
                isCritical = DiceRoller.Check(_attacker, DiceRollEnum.CriticalAttack, 20, _attacker.CharacterSheet.MeleeAttackBonus, _targetCharacter.CharacterSheet.ArmorClass);
            }


            // Do damage
            var nrOfDamageRolls = isCritical ? _attacker.CharacterSheet.EquipedWeapon.CriticalMultiplier : 1;
            for (var i = 0; i < nrOfDamageRolls; i++)
            {
                var damage = DiceRoller.Roll(_attacker, DiceRollEnum.Damage, _attacker.CharacterSheet.EquipedWeapon.DamageD, _attacker.CharacterSheet.CurrentMeleeDamageBonus);
                if (damage < 1)
                    damage = 1;
                GetEditableSheet(_targetCharacter).HitPoints -= damage;
            }
        }

        public int MaxRange
        {
            get 
            { 
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
