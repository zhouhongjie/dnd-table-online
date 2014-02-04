using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;

namespace DndTable.Core.Actions
{
    class AttackAction : BaseAction, IAttackAction
    {
        private ICharacter _attacker;

        internal AttackAction(ICharacter attacker)
        {
            _attacker = attacker;
        }

        public override ActionTypeEnum Type
        {
            get { return ActionTypeEnum.Standard; }
        }

        public override string Description
        {
            get
            {
                if (_attacker.CharacterSheet.EquipedWeapon == null)
                    return "Unarmed attack";
                if (_attacker.CharacterSheet.EquipedWeapon.IsRanged)
                    return "Ranged attack";

                return "Melee attack";
            }
        }

        public override void Do()
        {
            if (_targetCharacter == null)
                throw new InvalidOperationException("Character target expected");

            // Has melee weapon?
            if ((_attacker.CharacterSheet.EquipedWeapon == null))
                throw new ArgumentException("attacker has no equiped weapon");

            // Check max range
            var rangeRounded = GetTilesDistance(_attacker.Position, _targetCharacter.Position);
            if (rangeRounded > MaxRange)
                throw new InvalidOperationException("Out of range: should have been checked before calling this method");

            Register();


            // Check hit
            var check = DiceRoller.RollAttack(
                _attacker, 
                DiceRollEnum.Attack,
                _targetCharacter.CharacterSheet.GetCurrentAttackBonus((int)rangeRounded * 5),  // Convert tiles to feet
                _targetCharacter.CharacterSheet.ArmorClass,
                20 - _targetCharacter.CharacterSheet.EquipedWeapon.CriticalRange);

            if (!check.Success)
                return;

            // Critical hit
            bool isCritical = false;
            if (check.IsThreat)
            {
                isCritical = DiceRoller.Check(
                    _attacker, 
                    DiceRollEnum.CriticalAttack, 
                    20,
                    _attacker.CharacterSheet.GetCurrentAttackBonus((int)rangeRounded * 5), 
                    _targetCharacter.CharacterSheet.ArmorClass);
            }


            // Do damage
            var nrOfDamageRolls = isCritical ? _attacker.CharacterSheet.EquipedWeapon.CriticalMultiplier : 1;
            for (var i = 0; i < nrOfDamageRolls; i++)
            {
                var damage = DiceRoller.Roll(
                    _attacker, 
                    DiceRollEnum.Damage, 
                    _attacker.CharacterSheet.EquipedWeapon.DamageD, 
                    _attacker.CharacterSheet.GetCurrentDamageBonus());

                if (damage < 1)
                    damage = 1;

                GetEditableSheet(_targetCharacter).HitPoints -= damage;
            }
        }

        public int MaxRange
        {
            get 
            {
                if (_attacker.CharacterSheet.EquipedWeapon != null && _attacker.CharacterSheet.EquipedWeapon.IsRanged)
                {
                    // TODO: implement in weapon stats?
                    // Thrown weapons: range increment x5
                    // Projectile weapons: range increment x10

                    // x / 5 (feet to tiles) 

                    return (int) Math.Floor(_attacker.CharacterSheet.EquipedWeapon.RangeIncrement*2.0);
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
