using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;

namespace DndTable.Core.Actions
{
    class RangeAttackAction : BaseAction, IAttackAction
    {
        private ICharacter _attacker;

        internal RangeAttackAction(ICharacter attacker)
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
            if ((_attacker.CharacterSheet.EquipedWeapon == null) || !_attacker.CharacterSheet.EquipedWeapon.IsRanged)
                throw new ArgumentException("attacker has no range equiped weapon");

            Register();

            var range = GetDistance(_attacker.Position, _targetCharacter.Position);

            // Check hit
            if (!DiceRoller.Check(_attacker, DiceRollEnum.Attack, 20, _attacker.CharacterSheet.GetRangedAttackBonus(range), _targetCharacter.CharacterSheet.ArmorClass))
                return;

            // TODO: Check crit failure

            // TODO: Check crit


            // Do damage
            var damage = DiceRoller.Roll(_attacker, DiceRollEnum.Damage, _attacker.CharacterSheet.EquipedWeapon.DamageD, _attacker.CharacterSheet.CurrentRangeDamageBonus);
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

        public int MaxRange
        {
            get
            {
                // TODO: implement in weapon stats?
                // Thrown weapons: range increment x5
                // Projectile weapons: range increment x10

                // x / 5 (feet to tiles) 

                return (int)Math.Floor(_attacker.CharacterSheet.EquipedWeapon.RangeIncrement*2.0);
            }
        }

        public int MinRange { get { return 0; } }
    }
}
