using System;

namespace DndTable.Core.Characters
{
    public class CharacterSheet : ICharacterSheet
    {
        public string Name { get; internal set; }
        public CharacterRace Race { get; internal set; }

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

        public int ArmorClass
        {
            get
            {
                // TODO ... armour, bonusses, touch, flatfooted, ...

                var result = 10;

                // Add dex (not flat footed, ...)
                result += GetAbilityBonus(Dexterity);

                // Add size modifier
                result += SizeModifier;

                // Add armor (not touch, ...)
                if (EquipedArmor != null)
                    result += EquipedArmor.ArmorBonus;

                // RoundInfo
                result += CurrentRoundInfo.ArmorBonus;

                return result;
            }
        }

        public int Initiative { get; internal set; }

        public int Speed { get; internal set; }

        public int SizeModifier { get; internal set; }

        public int BaseAttackBonus { get; internal set; }

        private int MeleeAttackBonus
        {
            get { return BaseAttackBonus + SizeModifier + GetAbilityBonus(Strength); }
        }

        private int GetRangedAttackBonus(int range)
        {
            if (range >= EquipedWeapon.RangeIncrement)
            {
                // TODO: calculate range penalty
                throw new NotImplementedException();
            }
            var rangePenalty = 0;

            return BaseAttackBonus + SizeModifier + GetAbilityBonus(Dexterity) + rangePenalty;
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

        public int GetCurrentAttackBonus(int range)
        {
            // Unarmed
            if (EquipedWeapon == null)
                return MeleeAttackBonus + CurrentRoundInfo.AttackBonus;
            // Ranged
            if (EquipedWeapon.IsRanged)
                return GetRangedAttackBonus(range) + CurrentRoundInfo.AttackBonus;
            
            // Melee
            return MeleeAttackBonus + CurrentRoundInfo.AttackBonus;
        }

        public int GetCurrentDamageBonus()
        {
            // TODO: weapon bonus
            // TODO: weapon focus
            // ...

            // Unarmed
            if (EquipedWeapon == null)
                return GetAbilityBonus(Strength);
            // Ranged
            if (EquipedWeapon.IsRanged)
                return 0;

            // Melee
            return GetAbilityBonus(Strength);
        }

        public int GetCurrentSpeed()
        {
            // TODO: encumbrance

            if (EquipedArmor == null)
                return Speed;

            return EquipedArmor.AdjustedSpeed(Speed);
        }

        private static int GetAbilityBonus(int baseAbiltyScore)
        {
            return (int)Math.Floor((baseAbiltyScore - 10) / 2.0);
        }

        private RoundInfo _currentRoundInfo = new RoundInfo();
        internal RoundInfo CurrentRoundInfo { get { return _currentRoundInfo; } }
    }
}
