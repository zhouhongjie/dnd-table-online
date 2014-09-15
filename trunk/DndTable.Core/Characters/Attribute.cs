using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Spells;

namespace DndTable.Core.Characters
{
    internal class Attribute
    {
        internal Attribute(string description)
        {
            Description = description;
            Buffs = new List<AttributeBuff>();
        }

        public string Description { get; private set; }
        public int BaseStat { get; private set; }

        public int GetValue()
        {
            CleanupAttributeBuffs();
            var attributeBuffs = CalculateAttributeBuffs();

            // Temp untill 'Buff' is removed
            var maxBuffValue = Math.Max(attributeBuffs, Buff);

            return BaseStat + maxBuffValue;
        }

        private void CleanupAttributeBuffs()
        {
            Buffs = Buffs.Where(buff => !buff.ParentEffect.IsExpired).ToList();
        }

        private int CalculateAttributeBuffs()
        {
            var total = 0;

            foreach (var buff in Buffs)
            {
                if (buff.ParentEffect.IsExpired)
                    throw new InvalidOperationException("Buff should have been clean up");

                // TODO: take into account different BuffTypes
                if (buff.BuffValue > total)
                    total = buff.BuffValue;
            }

            return total;
        }

        public void SetValue(int value)
        {
            if (Buff != 0)
                throw new NotSupportedException("TODO: review manipulation of attributes => this can be confusing when there are buffs active = currently only used for initialization");

            BaseStat = value;
        }

        public int GetAbilityBonus()
        {
            return (int)Math.Floor((GetValue() - 10) / 2.0);
        }

        public void AddBuff(int buffValue)
        {
            if (buffValue > Buff)
                Buff = buffValue;
        }

        public void AddBuff(AttributeBuff buff)
        {
        }

        public void ClearBuff()
        {
            Buff = 0;
        }

        // TODO: list of buff/debuff objects to incorporate duration & type (enhancement, luck, ...)
        private int Buff { get; set; }
        private List<AttributeBuff> Buffs { get; set; } 
    }
}
