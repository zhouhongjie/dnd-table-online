using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core.Characters
{
    internal class Attribute
    {
        internal Attribute(string description)
        {
            Description = description;
        }

        public string Description { get; private set; }
        public int BaseStat { get; private set; }

        public int GetValue()
        {
            return BaseStat + Buff;
        }

        public void SetValue(int value)
        {
            if (BaseStat != 0)
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

        // TODO: list of buff/debuff objects to incorporate duration & type (enhancement, luck, ...)
        private int Buff { get; set; }
    }
}
