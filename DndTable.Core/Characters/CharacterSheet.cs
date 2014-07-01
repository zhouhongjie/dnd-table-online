using System;
using System.Collections.Generic;
using DndTable.Core.Armors;
using DndTable.Core.Items;
using DndTable.Core.Log;
using DndTable.Core.Spells;
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

        public int Strength { get { return StrengthAttribute.GetValue(); } set { StrengthAttribute.SetValue(value); } }
        public int Dexterity { get { return DexterityAttribute.GetValue(); } set { DexterityAttribute.SetValue(value); } }
        public int Constitution { get { return ConstitutionAttribute.GetValue(); } set { ConstitutionAttribute.SetValue(value); } }
        public int Intelligence { get { return IntelligenceAttribute.GetValue(); } set { IntelligenceAttribute.SetValue(value); } }
        public int Wisdom { get { return WisdomAttribute.GetValue(); } set { WisdomAttribute.SetValue(value); } }
        public int Charisma { get { return CharismaAttribute.GetValue(); } set { CharismaAttribute.SetValue(value); } }

        public int Fortitude { get; internal set; }
        public int Reflex { get; internal set; }
        public int Will { get; internal set; }

        public int HitPoints { get; internal set; }
        public int MaxHitPoints { get; internal set;  }

        public int Speed { get; internal set; }
        public int SizeModifier { get; internal set; }

        public int BaseAttackBonus { get; internal set; }

        public int NaturalArmor { get; internal set; }

        #region inventory
        private readonly List<IPotion> _potions = new List<IPotion>();
        private readonly List<IWeapon> _weapons = new List<IWeapon>();
        private readonly List<ISpell> _spells = new List<ISpell>();
        public List<IPotion> Potions { get { return _potions; } }
        public List<IWeapon> Weapons { get { return _weapons; } }
        public List<ISpell> Spells { get { return _spells; } }
        #endregion

        #region attributes // TODO: list of buff/debuff objects to incorporate duration & type (enhancement, luck, ...)
        internal Attribute StrengthAttribute = new Attribute("Strength");
        internal Attribute DexterityAttribute = new Attribute("Dexterity");
        internal Attribute ConstitutionAttribute = new Attribute("Constitution");
        internal Attribute IntelligenceAttribute = new Attribute("Intelligence");
        internal Attribute WisdomAttribute = new Attribute("Wisdom");
        internal Attribute CharismaAttribute = new Attribute("Charisma");
        #endregion

        private readonly HashSet<ConditionEnum> _conditions = new HashSet<ConditionEnum>();
        public HashSet<ConditionEnum> Conditions { get { return _conditions; } }

        internal bool HasCondition(ConditionEnum condition)
        {
            return _conditions.Contains(condition);
        }
        
        internal bool RemoveCondition(ConditionEnum condition)
        {
            return _conditions.Remove(condition);
        }
        
        private int GetMeleeAttackBonus(Calculator.CalculatorPropertyContext context)
        {
            return context.Use(BaseAttackBonus, "BaseAttackBonus") +
                   context.Use(SizeModifier, "SizeModifier") +
                   context.UseAbilityBonus(StrengthAttribute);
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
                    context.UseAbilityBonus(DexterityAttribute) +
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
            if (HasCondition(ConditionEnum.Sleeping))
                return false;

            // TEMP (I know this is not correct)
            return HitPoints > 0;
        }

        public void ApplyDamage(int damage)
        {
            HitPoints -= damage;

            // Awaken
            RemoveCondition(ConditionEnum.Sleeping);
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
                    return context.UseAbilityBonus(StrengthAttribute);
                // Ranged
                if (EquipedWeapon.IsRanged)
                    return 0;

                // Melee
                return context.UseAbilityBonus(StrengthAttribute);
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

        public int GetCurrentArmorClass()
        {
            using (var context = Calculator.CreatePropertyContext("ArmorClass"))
            {
                // TODO ... armour, bonusses, touch, flatfooted, ...

                var result = 10;

                // Add dex (not flat footed, ...)
                result += context.UseAbilityBonus(DexterityAttribute);

                // Add size modifier
                result += context.Use(SizeModifier, "Size");

                // Natural armor
                result += NaturalArmor;

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
                return context.UseAbilityBonus(DexterityAttribute);
            }
        }

        public int GetArcaneSpellDC(int spellLevel)
        {
            using (var context = Calculator.CreatePropertyContext("ArcaneSpellDC"))
            {
                return context.UseAbilityBonus(IntelligenceAttribute) +
                       context.Use(spellLevel, "SpellLevel") +
                       context.Use(10, "10");
            }
        }


        private RoundInfo _currentRoundInfo = new RoundInfo();
        internal RoundInfo CurrentRoundInfo { get { return _currentRoundInfo; } }


    }
}
