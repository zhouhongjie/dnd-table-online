using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;

namespace DndTable.Core.Actions
{
    class MeleeAttackAction : BaseAction, IMeleeAttackAction
    {
        private IDiceRoller _diceRoller;
        private ICharacter _attacker;

        internal MeleeAttackAction(IDiceRoller diceRoller, ICharacter attacker)
        {
            _diceRoller = diceRoller;
            _attacker = attacker;
        }

        public override void Do()
        {
            if (_targetCharacter == null)
                throw new InvalidOperationException("Character target expected");

            // Has weapon?
            if (_attacker.CharacterSheet.EquipedWeapon == null)
                throw new ArgumentException("attacker has no equiped weapon");

            // Can reach


            // Check hit
            if (!_diceRoller.Check(DiceRollEnum.Attack, 20, _attacker.CharacterSheet.MeleeAttackBonus, _targetCharacter.CharacterSheet.ArmorClass))
                return;

            // Check crit failure

            // Check crit

            // Do damage
            var damage = _diceRoller.Roll(DiceRollEnum.Damage, _attacker.CharacterSheet.EquipedWeapon.DamageD, _attacker.CharacterSheet.CurrentMeleeDamageBonus);
            if (damage < 1)
                damage = 1;

            GetEditableSheet(_targetCharacter).HitPoints -= damage;
        }
    }
}
