﻿using System;
using System.Collections.Generic;
using DndTable.Core.Armors;
using DndTable.Core.Factories;
using DndTable.Core.Items;
using DndTable.Core.Log;
using DndTable.Core.Spells;
using DndTable.Core.Weapons;

namespace DndTable.Core.Characters
{
    public class CharacterSheet : ICharacterSheet
    {
        internal CharacterSheet()
        {
            _conditions = new CharacterConditions(this);
            Level = new Dictionary<CharacterClass, int>();

            Size = SizeEnum.Medium;

            FortitudeProperty = new SaveProperty("Fortitude", ConstitutionAttribute);
            ReflexProperty = new SaveProperty("Reflex", DexterityAttribute);
            WillProperty = new SaveProperty("Will", WisdomAttribute);

            HpProperty = new HitpointsProperty("Hitpoints", ConstitutionAttribute, Level);
            MaxHpProperty = new HitpointsProperty("MaxHitpoints", ConstitutionAttribute, Level);
        }

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

        public Dictionary<CharacterClass, int> Level { get; private set; }

        public int Strength { get { return StrengthAttribute.GetValue(); } set { StrengthAttribute.SetValue(value); } }
        public int Dexterity { get { return DexterityAttribute.GetValue(); } set { DexterityAttribute.SetValue(value); } }
        public int Constitution { get { return ConstitutionAttribute.GetValue(); } set { ConstitutionAttribute.SetValue(value); } }
        public int Intelligence { get { return IntelligenceAttribute.GetValue(); } set { IntelligenceAttribute.SetValue(value); } }
        public int Wisdom { get { return WisdomAttribute.GetValue(); } set { WisdomAttribute.SetValue(value); } }
        public int Charisma { get { return CharismaAttribute.GetValue(); } set { CharismaAttribute.SetValue(value); } }

        public int Fortitude { get { return FortitudeProperty.GetValue(); } }
        public int Reflex { get { return ReflexProperty.GetValue(); } }
        public int Will { get { return WillProperty.GetValue(); } }

        internal SaveProperty FortitudeProperty { get; private set; }
        internal SaveProperty ReflexProperty { get; private set; }
        internal SaveProperty WillProperty { get; private set; }

        public int HitPoints { get { return HpProperty.GetValue(); } }
        public int MaxHitPoints { get { return MaxHpProperty.GetValue(); } }

        internal HitpointsProperty HpProperty { get; private set; }
        internal HitpointsProperty MaxHpProperty { get; private set; }

        public int Speed { get; internal set; }
        public SizeEnum Size { get; internal set; }
        public int SizeModifier 
        { 
            get
            {
                if (Size == SizeEnum.Small)
                    return 1;
                if (Size == SizeEnum.Medium)
                    return 0;
                if (Size == SizeEnum. Large)
                    return -1;
                throw new NotImplementedException("TODO: SizeModifier for " + Size);
            } 
        }

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

        private void ClearAttributeBuffs()
        {
            StrengthAttribute.ClearBuff();
            DexterityAttribute.ClearBuff();
            ConstitutionAttribute.ClearBuff();
            IntelligenceAttribute.ClearBuff();
            WisdomAttribute.ClearBuff();
            CharismaAttribute.ClearBuff();
        }
        #endregion

        #region Conditions
        private readonly CharacterConditions _conditions;
        public ICharacterConditions Conditions { get { return _conditions; } }
        internal CharacterConditions EditableConditions { get { return _conditions; } }

        public bool LooseDexBonusToAC()
        {
            if (EditableConditions.IsHelpless)
                return true;

            // TODO: negated by uncanny-dodge ability
            if (EditableConditions.IsFlatFooted)
                return true;

            // TODO: other checks
            return false;
        }

        #endregion

        #region Immunities
        private readonly  CharacterImmunities _immunities = new CharacterImmunities();
        public ICharacterImmunities Immunities { get { return _immunities; } }
        internal CharacterImmunities EditableImmunities { get { return _immunities; } }
        #endregion

        #region Effects
        private readonly List<BaseEffect> _effects = new List<BaseEffect>();
        internal void AddAndApplyEffect(BaseEffect effect)
        {
            effect.Apply();
            _effects.Add(effect);
            
        }
        // internal void AdvanceOneRound() ??
        // CancelEffectsOnDamage
        internal void ApplyEffectsForThisRound()
        {
            RemoveExpiredEffects();
            ApplyEffects();
        }

        private void ApplyEffects()
        {
            ClearAttributeBuffs();
            EditableConditions.ClearEffects();

            foreach (var effect in _effects)
            {
                effect.Apply();
            }
        }

        private void RemoveExpiredEffects()
        {
            var allEffects = new List<BaseEffect>(_effects);
            foreach (var effect in allEffects)
            {
                // TODO: THIS IS NOT THE CORRECT WAY TO HANDLE EFFECT DURATIONS
                // => still thinking about the correct way
                // Problem = when does an effect duration start? (effect should have it's own initiave counter)
                if (!effect.DecreaseDurationAndCheck())
                {
                    _effects.Remove(effect);
                    effect.IsExpired = true;
                }
            }
        }

        private void CancelEffectsOnDamage()
        {
            // Remove cancelled effects
            var allEffects = new List<BaseEffect>(_effects);
            foreach (var effect in allEffects)
            {
                if (effect.CancelOnDamage)
                {
                    _effects.Remove(effect);
                    effect.IsExpired = true;
                }
            }

            // Re-apply effects to push the cancel to the char sheet
            ApplyEffects();
        }

        #endregion

        #region Feats

        public bool CanSneakAttack { get; internal set; }
        public bool ImprovedInitiative { get; internal set; }

        #endregion

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
            if (EditableConditions.IsHelpless)
                return false;

            // TEMP (I know this is not correct)
            // should also be handled by Conditions
            return HitPoints > 0;
        }

        public void ApplyDamage(int damage)
        {
            HpProperty.BaseValue -= damage;

            // Cancel effects responsive to damage
            CancelEffectsOnDamage();

            // SHOULD NO LONGER BE NECESSARY!
            // Awaken
            //EditableConditions.IsSleeping = false;
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

                var currentWeapon = GetCurrentWeapon();

                // NaturalWeapons
                if (HasNaturalWeapons)
                {
                    return context.Use(NaturalWeapons[0].Attack, NaturalWeapons[0].Description + " bonus") +
                           CurrentRoundInfo.UseAttackBonus(context) +
                           context.Use(isFlanking ? 2 : 0, "Flanking");
                }

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

        internal AttackRollStatistics GetCurrentDamageRoll()
        {
            if (HasNaturalWeapons)
            {
                // TODO: multi-attack
                var weapon = NaturalWeapons[0];
                return new AttackRollStatistics(weapon.NrOfDamageDice, weapon.DamageD, weapon.DamageBonus, 0, 2, !weapon.IsRanged);
            }
            else
            {
                // TODO: weapon can have multiple rolls as well
                if (EquipedWeapon == null)
                    throw new NotImplementedException("TODO: unarmed attack");

                return new AttackRollStatistics(EquipedWeapon.NrOfDamageDice,
                                                EquipedWeapon.DamageD,
                                                GetCurrentDamageBonus(),
                                                EquipedWeapon.CriticalRange,
                                                EquipedWeapon.CriticalMultiplier,
                                                !EquipedWeapon.IsRanged);
            }
        }

        private int GetCurrentDamageBonus()
        {
            if (HasNaturalWeapons)
                throw new InvalidOperationException("shouldn't use DamageBonus for natural weapons");

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
                if (!LooseDexBonusToAC())
                    result += context.UseAbilityBonus(DexterityAttribute);

                // Add size modifier
                result += context.Use(SizeModifier, "Size");

                // Natural armor
                result += context.Use(NaturalArmor, "NaturalArmor");

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
                var initiative = context.UseAbilityBonus(DexterityAttribute);

                if (ImprovedInitiative)
                    initiative += context.Use(4, "ImprovedInitiative");

                return initiative;
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


        public int GetMaxNrOfAttacks()
        {
            // http://rpg.stackexchange.com/questions/22047/how-do-multiple-attacks-with-natural-weapons-work

            if (_naturalWeapons.Count > 1)
                return _naturalWeapons.Count;

            // TODO: also for manufactured weapons
            return 1;
        }

        private RoundInfo _currentRoundInfo = new RoundInfo();
        internal RoundInfo CurrentRoundInfo { get { return _currentRoundInfo; } }

        private List<NaturalWeapon> _naturalWeapons = new List<NaturalWeapon>();
        internal List<NaturalWeapon> NaturalWeapons { get { return _naturalWeapons; } }

        public bool HasNaturalWeapons { get { return NaturalWeapons.Count > 0; }}

        public IWeapon GetCurrentWeapon()
        {
            // TODO: multi-attacks, natural & normal mixed weapons, unarmed, ...
            if (HasNaturalWeapons)
                return NaturalWeapons[CurrentRoundInfo.AttackCounter];
            if (EquipedWeapon == null)
                throw new NotSupportedException("TODO: unarmed combat");
            return EquipedWeapon;
        }

    }


    public class AttackRollStatistics
    {
        public AttackRollStatistics( int nrOfDice, int d, int bonus, int criticalRange, int criticalMultiplier, bool isMelee)
        {
            NrOfDice = nrOfDice;
            D = d;
            Bonus = bonus;
            CriticalRange = criticalRange;
            CriticalMultiplier = criticalMultiplier;
        }

        public int NrOfDice { get; private set; }
        public int D { get; private set; }
        public int Bonus { get; private set; }   
        public int CriticalRange { get; private set; }   
        public int CriticalMultiplier { get; private set; }   
        public bool IsMelee { get; private set; }   
    }
}

