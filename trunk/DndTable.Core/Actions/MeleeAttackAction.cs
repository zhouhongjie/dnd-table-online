﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;

namespace DndTable.Core.Actions
{
    class MeleeAttackAction : BaseAction, IMeleeAttackAction
    {
        private ICharacter _attacker;

        internal MeleeAttackAction(ICharacter attacker)
        {
            _attacker = attacker;
        }

        public override void Do()
        {
            if (_targetCharacter == null)
                throw new InvalidOperationException("Character target expected");

            // Has melee weapon?
            if ((_attacker.CharacterSheet.EquipedWeapon == null) || _attacker.CharacterSheet.EquipedWeapon.IsRanged)
                throw new ArgumentException("attacker has no melee equiped weapon");

            //var range = GetDistance(_attacker.Position, _targetCharacter.Position);

            // Can reach


            // Check hit
            if (!DiceRoller.Check(DiceRollEnum.Attack, 20, _attacker.CharacterSheet.MeleeAttackBonus, _targetCharacter.CharacterSheet.ArmorClass))
                return;

            // TODO: Check crit failure

            // TODO: Check crit


            // Do damage
            var damage = DiceRoller.Roll(DiceRollEnum.Damage, _attacker.CharacterSheet.EquipedWeapon.DamageD, _attacker.CharacterSheet.CurrentMeleeDamageBonus);
            if (damage < 1)
                damage = 1;

            GetEditableSheet(_targetCharacter).HitPoints -= damage;
        }

        private static int GetDistance(Position position1, Position position2)
        {
            var dx = position1.X - position2.X;
            var dy = position1.Y - position2.Y;

            return (int)Math.Sqrt(dx*dx + dy*dy);
        }
    }
}
