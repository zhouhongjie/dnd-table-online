using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Log;

namespace DndTable.Core.Actions
{
    class AttackAction : BaseAction, IAttackAction
    {
        private ICharacter _attacker;

        internal AttackAction(ICharacter attacker)
            : base(attacker)
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
            using (var context = Calculator.CreateActionContext(this))
            {
                _Do(context);
            }
        }

        private void _Do(Calculator.CalculatorActionContext context)
        {
            if (_targetCharacter == null)
                throw new InvalidOperationException("Character target expected");

            // Has melee weapon?
            if ((_attacker.CharacterSheet.EquipedWeapon == null))
                throw new ArgumentException("attacker has no equiped weapon");

            // Check max range
            var rangeRounded = MathHelper.GetTilesDistance(_attacker.Position, _targetCharacter.Position);
            if (rangeRounded > MaxRange)
                throw new InvalidOperationException("Out of range: should have been checked before calling this method");

            Register();


            // Check counter AttackOfOpportunity
            if (ProvokesAttackOfOpportunity())
            {
                HandleAttackOfOpportunity(context);

                // TODO: CharacterSheet function to check if a char can still act
                if (_attacker.CharacterSheet.HitPoints <= 0)
                    return;
            }


            // Check hit
            var check = DiceRoller.RollAttack(
                _attacker, 
                DiceRollEnum.Attack,
                _attacker.CharacterSheet.GetCurrentAttackBonus((int)rangeRounded * 5, IsFlanking()),  // Convert tiles to feet
                _targetCharacter.CharacterSheet.GetCurrentArmorClass(),
                20 - _attacker.CharacterSheet.EquipedWeapon.CriticalRange);

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
                    _attacker.CharacterSheet.GetCurrentAttackBonus((int)rangeRounded * 5, IsFlanking()),
                    _targetCharacter.CharacterSheet.GetCurrentArmorClass());
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

        private bool IsFlanking()
        {
            return ActionHelper.IsFlanking(_attacker, _targetCharacter, this.Encounter.Participants);
        }

        private void HandleAttackOfOpportunity(Calculator.CalculatorActionContext context)
        {
            foreach (var participant in this.Encounter.Participants)
            {
                // no ally bashing
                if (participant.CharacterSheet.FactionId == _attacker.CharacterSheet.FactionId)
                    continue;

                if (ActionHelper.IsInThreatenedArea(_attacker, participant))
                {
                    // check participant already did an AoO
                    // TODO: possibly multiple AoO's (combat reflexes)
                    var roundInfo = Encounter.GetRoundInfo(participant);
                    if (roundInfo.AttackOfOpportunityCounter > 0)
                        continue;

                    // Increase counter
                    roundInfo.AttackOfOpportunityCounter++;

                    // handle AttackAction of participant
                    // TODO: requires UI interaction !!!!!! (for the moment auto attack)

                    // Note: AoO is always a MeleeAttack in the proper range (otherwise ThreatenedArea is wrong)
                    context.AoO(participant, _attacker);
                    var attackOfOpportunity = ActionFactory.MeleeAttack(participant);
                    attackOfOpportunity.Target(_attacker);
                    attackOfOpportunity.Do();
                }
            }
        }

        private bool ProvokesAttackOfOpportunity()
        {
            return _attacker.CharacterSheet.EquipedWeapon.IsRanged;
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
