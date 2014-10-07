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
                var label = Executer.CharacterSheet.GetCurrentWeapon().Description;
                if (Executer.CharacterSheet.GetCurrentWeapon().IsRanged)
                    label += " (R)";
                return label;
            }
        }

        public override void Do()
        {
            using (var context = Calculator.CreateActionContext(this))
            {
                _Do(context);

                // Keep track of multiple attacks: make sure this is done last (counter is also used to get the CurrentWeapon of the Character)
                CharacterSheet.GetEditableSheet(Executer).CurrentRoundInfo.AttackCounter++;
            }
        }

        private void _Do(Calculator.CalculatorActionContext context)
        {
            if (_targetCharacter == null)
                throw new InvalidOperationException("Character target expected");

            // Weapon equiped
            if (Executer.CharacterSheet.GetCurrentWeapon() == null)
                throw new InvalidOperationException("attacker has no equiped weapon");

            // Needs reload?
            if (Executer.CharacterSheet.GetCurrentWeapon().NeedsReload)
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
            Executer.CharacterSheet.GetCurrentWeapon().Use();

            // don't use the stats of the EquipedWeapon
            // => use the DamageRollStatistics
            var damageRollInfo = CharacterSheet.GetEditableSheet(Executer).GetCurrentDamageRoll();

            // Check hit
            var check = DiceRoller.RollAttack(
                Executer,
                DiceRollEnum.Attack,
                Executer.CharacterSheet.GetCurrentAttackBonus((int)rangeRounded * 5, IsFlanking()),  // Convert tiles to feet
                _targetCharacter.CharacterSheet.GetCurrentArmorClass(),
                20 - damageRollInfo.CriticalRange);

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
            {
                if (isCritical && _targetCharacter.CharacterSheet.Immunities.ImmuneToCriticalHits)
                {
                    Logger.Singleton.LogImmunity(_targetCharacter.CharacterSheet, "CriticalHits");
                    isCritical = false;
                }

                // Normal damage + critical
                var damage = 0;
                var nrOfDamageRolls = isCritical ? damageRollInfo.CriticalMultiplier : 1;
                for (var i = 0; i < nrOfDamageRolls; i++)
                {
                    var currentDamage = DiceRoller.Roll(
                        Executer,
                        DiceRollEnum.Damage,
                        damageRollInfo.NrOfDice,
                        damageRollInfo.D,
                        damageRollInfo.Bonus);

                    // Check damage reductions
                    currentDamage = CheckDamageReductions(currentDamage);

                    // TODO: verify -> check here or @ the end
                    if (currentDamage < 1)
                        currentDamage = 1;

                    damage += currentDamage;
                }

                // SneakAttack 
                if (CanSneakAttack(Executer, _targetCharacter))
                {
                    var currentDamage = DiceRoller.Roll(
                        Executer,
                        DiceRollEnum.SneakAttack,
                        1, // TODO: higher lvl sneaks attacks
                        6,
                        0);

                    damage += currentDamage;
                }
                CharacterSheet.GetEditableSheet(_targetCharacter).ApplyDamage(damage);

                // Special effect
                (Executer.CharacterSheet.GetCurrentWeapon() as BaseWeapon).ApplyEffect(_targetCharacter, DiceRoller);
            }
        }

        private int CheckDamageReductions(int damage)
        {
            if (_targetCharacter.CharacterSheet.Immunities.HalfDamageFromPiercing
                && Executer.CharacterSheet.GetCurrentWeapon().DamageTypes.Contains(WeaponDamageTypeEnum.Piercing))
            {
                damage = (int)Math.Ceiling(damage / 2.0);
                Logger.Singleton.LogImmunity(_targetCharacter.CharacterSheet, "HalfDamageFromPiercing => " + damage + " damage");
            }

            if (_targetCharacter.CharacterSheet.Immunities.HalfDamageFromSlashing
                && Executer.CharacterSheet.GetCurrentWeapon().DamageTypes.Contains(WeaponDamageTypeEnum.Slashing))
            {
                damage = (int)Math.Ceiling(damage / 2.0);
                Logger.Singleton.LogImmunity(_targetCharacter.CharacterSheet, "HalfDamageFromSlashing => " + damage + " damage");
            }

            return damage;
        }

        private bool CanSneakAttack(ICharacter attacker, ICharacter target)
        {
            // <= 30ft
            var rangeRounded = MathHelper.GetTilesDistance(Executer.Position, _targetCharacter.Position);
            if (rangeRounded > 6)
                return false;

            var attackerSheet = CharacterSheet.GetEditableSheet(attacker);
            var targetSheet = CharacterSheet.GetEditableSheet(target);

            if (!attackerSheet.CanSneakAttack)
                return false;

            // TODO: check target has concealment

            bool canSneak =
                targetSheet.LooseDexBonusToAC() ||
                IsFlanking();

            // Immunity
            if (canSneak && targetSheet.Immunities.ImmuneToSneakAttacks)
            {
                Logger.Singleton.LogImmunity(targetSheet, "SneakAttacks");
                return false;
            }

            return canSneak;
        }

        private bool IsFlanking()
        {
            return ActionHelper.IsFlanking(Executer, _targetCharacter, this.Encounter.Participants);
        }

        private bool ProvokesAttackOfOpportunity()
        {
            return Executer.CharacterSheet.GetCurrentWeapon().ProvokesAoO;
        }

        public int MaxRange
        {
            get 
            {
                if (Executer.CharacterSheet.GetCurrentWeapon().IsRanged)
                {
                    // TODO: implement in weapon stats?
                    // Thrown weapons: range increment x5
                    // Projectile weapons: range increment x10

                    // x / 5 (feet to tiles) 

                    return (int)Math.Floor(Executer.CharacterSheet.GetCurrentWeapon().RangeIncrement * 2.0);
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
