using System;
using System.Collections.Generic;
using DndTable.Core.Armors;
using DndTable.Core.Items;
using DndTable.Core.Log;
using DndTable.Core.Weapons;

namespace DndTable.Core.Characters
{
    public class CharacterSheet : ICharacterSheet
    {
        internal static CharacterSheet GetEditableSheet(ICharacter character)
        {
            var sheet = character.CharacterSheet as CharacterSheet;
            if (sheet == null)
                throw new ArgumentException();
            return sheet;
        }


        public string Name { get; internal set; }
        public CharacterRace Race { get; internal set; }
        public int FactionId { get; internal set; }

        public int Strength { get; internal set; }
        public int Dexterity { get; internal set; }
        public int Constitution { get; internal set; }
        public int Intelligent { get; internal set; }
        public int Wisdom { get; internal set; }
        public int Charisma { get; internal set; }

        public int Fortitude { get; internal set; }
        public int Reflex { get; internal set; }
        public int Will { get; internal set; }

        public int HitPoints { get; internal set; }
        public int MaxHitPoints { get; internal set;  }

        public int Speed { get; internal set; }
        public int SizeModifier { get; internal set; }

        public int BaseAttackBonus { get; internal set; }

        #region inventory
        private readonly List<IPotion> _potions = new List<IPotion>();
        private readonly List<IWeapon> _weapons = new List<IWeapon>();
        public List<IPotion> Potions { get { return _potions; } }
        public List<IWeapon> Weapons { get { return _weapons; } }
        #endregion

        private int GetMeleeAttackBonus(Calculator.CalculatorPropertyContext context)
        {
            return context.Use(BaseAttackBonus, "BaseAttackBonus") +
                   context.Use(SizeModifier, "SizeModifier") +
                   context.Use(GetAbilityBonus(Strength), "Strength");
        }

        private int GetRangedAttackBonus(int range, Calculator.CalculatorPropertyContext context)
        {
            var rangePenalty = 0;

            if (range >= EquipedWeapon.RangeIncrement)
            {
                // TODO: MaxRange
                // Difference between Thrown & Projectile
                // * thrown = max range of 5 range increments
                // * projectile = max range of 10 range increments

                var nrOfRangeIncrements = (int) Math.Floor((double) range/(double) EquipedWeapon.RangeIncrement);
                rangePenalty = nrOfRangeIncrements*-2;
            }

            return context.Use(BaseAttackBonus, "BaseAttackBonus") +
                    context.Use(SizeModifier, "Size") +
                    context.Use(GetAbilityBonus(Dexterity), "Dexterity") +
                    context.Use(rangePenalty, "RangePenalty");
        }


        public IArmor EquipedArmor { get; internal set; }

        public IWeapon EquipedWeapon { get; internal set; }

        /// <summary>
        /// Unconscious: http://www.dandwiki.com/wiki/SRD:Unconscious
        /// Dead: http://www.dandwiki.com/wiki/SRD:Dead
        /// </summary>
        /// <returns></returns>
        public bool CanAct()
        {
            // TEMP (I know this is not correct)
            return HitPoints > 0;
        }

        public int GetCurrentAttackBonus(int range, bool isFlanking)
        {
            using (var context = Calculator.CreatePropertyContext("AttackBonus"))
            {
                // TODO: Unarmed
                //if (EquipedWeapon == null)
                //{
                //    return GetMeleeAttackBonus(context) +
                //           CurrentRoundInfo.UseAttackBonus(context);
                //}

                // Ranged
                if (EquipedWeapon != null && EquipedWeapon.IsRanged)
                {
                    return GetRangedAttackBonus(range, context) +
                           CurrentRoundInfo.UseAttackBonus(context);
                }

                // Melee
                return GetMeleeAttackBonus(context) +
                       CurrentRoundInfo.UseAttackBonus(context) +
                       context.Use(isFlanking ? 2 : 0, "Flanking");
            }
        }

        public int GetCurrentDamageBonus()
        {
            using (var context = Calculator.CreatePropertyContext("DamageBonus"))
            {
                // TODO: weapon bonus
                // TODO: weapon focus
                // ...

                // Unarmed
                if (EquipedWeapon == null)
                    return context.Use(GetAbilityBonus(Strength), "Strength");
                // Ranged
                if (EquipedWeapon.IsRanged)
                    return 0;

                // Melee
                return context.Use(GetAbilityBonus(Strength), "Strength");
            }
        }

        public int GetCurrentSpeed()
        {
            using (var context = Calculator.CreatePropertyContext("Speed"))
            {
                // TODO: encumbrance

                if (EquipedArmor == null)
                    return Speed;

                return EquipedArmor.AdjustedSpeed(Speed);
            }
        }

        private static int GetAbilityBonus(int baseAbiltyScore)
        {
            return (int)Math.Floor((baseAbiltyScore - 10) / 2.0);
        }

        public int GetCurrentArmorClass()
        {
            using (var context = Calculator.CreatePropertyContext("ArmorClass"))
            {
                // TODO ... armour, bonusses, touch, flatfooted, ...

                var result = 10;

                // Add dex (not flat footed, ...)
                result += context.Use(GetAbilityBonus(Dexterity), "Dexterity");

                // Add size modifier
                result += context.Use(SizeModifier, "Size");

                // Add armor (not touch, ...)
                if (EquipedArmor != null)
                    result += context.Use(EquipedArmor.ArmorBonus, "ArmorBonus");

                // RoundInfo
                result += CurrentRoundInfo.UseArmorBonus(context);

                return result;
            }
        }

        public int GetCurrentInitiative()
        {
            using (var context = Calculator.CreatePropertyContext("Initiative"))
            {
                // TODO: modifiers
                return context.Use(GetAbilityBonus(Dexterity), "Dexterity");
            }
        }


        private RoundInfo _currentRoundInfo = new RoundInfo();
        internal RoundInfo CurrentRoundInfo { get { return _currentRoundInfo; } }


    }
}
