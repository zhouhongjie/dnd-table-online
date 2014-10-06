using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;

namespace DndTable.Core.Actions
{
    /// <summary>
    /// http://www.dandwiki.com/wiki/SRD:Charge
    /// </summary>
    class ChargeAction : BaseAction, IAttackAction
    {
        private readonly ICharacter _attacker;
        private bool _isPartial;

        internal ChargeAction(ICharacter attacker, bool isPartial)
            : base(attacker)
        {
            _attacker = attacker;
            _isPartial = isPartial;
        }

        public int MaxRange
        {
            get
            {
                if (_isPartial)
                    return _attacker.CharacterSheet.GetCurrentSpeed()/5 + 1; // speed + 1 for attack (/5 for ft)

                return _attacker.CharacterSheet.GetCurrentSpeed()*2/5 + 1; // Double speed + 1 for attack (/5 for ft)
            }
        }

        public int MinRange
        {
            get { return 3; } // 10 foot + 1 for attack
        }

        public override ActionTypeEnum Type
        {
            // TO VERIFY: OK FOR PARTIAL CHARGE
            // As a follow-up, this action triggers an attack = Move + Attack = FullRound
            get { return ActionTypeEnum.MoveEquivalent; }
        }

        public override ActionCategoryEnum Category
        {
            get { return ActionCategoryEnum.Combat; }
        }

        public override string Description
        {
            get { return _isPartial ? "Partial charge" : "Charge"; }
        }

        public override void Do()
        {
            using (var context = Calculator.CreateActionContext(this))
            {
                _Do();
            }
        }

        private void _Do()
        {
            if (_targetCharacter == null)
                throw new InvalidOperationException("TargetCharacter required");

            if (_attacker.CharacterSheet.GetCurrentWeapon() != null && _attacker.CharacterSheet.GetCurrentWeapon().IsRanged)
                throw new NotSupportedException("Charging with a ranged weapon is not supported");

            // Calculate new position = straight line from _attacker to target - striking distance (= 1 tile for now)
            var newPosition = MathHelper.Go1TileInDirection(_targetCharacter.Position, _attacker.Position);

            // Check range
            //if (GetTilesDistance(newPosition, _attacker.Position) > 1)
            //    return false;


            // TODO: BUFFS SHOULD NOT BE APPLIED WHEN MOVE IS NOT EXECUTED
            // Buffs & penalties (before move = used for AoO's against _attacker)
            var roundInfo = this.Encounter.GetRoundInfo(_attacker);
            roundInfo.AttackBonus = 2;
            roundInfo.ArmorBonus = -2;

            // TODO: step-by-step move? to check AoO

            // Move
            if (!Board.MoveEntity(_attacker, newPosition))
                return;

            // Register move
            Register();

            // Do attack
            ActionFactory.MeleeAttack(_attacker).Target(_targetCharacter).Do();
        }

    }
}
