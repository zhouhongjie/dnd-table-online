using System.Collections.Generic;
using DndTable.Core.Characters;

namespace DndTable.Core.Spells
{
    internal class AttributeBuffEffect2 : BaseEffect
    {
        public List<AttributeBuff> AttributeBuffs { get; private set; } 

        public AttributeBuffEffect2(CharacterSheet sheet, int duration) : base(sheet, duration)
        {
            AttributeBuffs = new List<AttributeBuff>();
        }

        internal override void Apply()
        {
            // Do nothing
            // New mechanism => should work through AttributeBuffs = not applied but Calculated when needed in Attribute.Calculate
        }
    }

    internal class AttributeBuff
    {
        public AttributeBuffEffect2 ParentEffect { get; private set; }
        public int BuffValue { get; private set; }
        //internal BuffTypeEnum BuffType { get; private set; }

        public AttributeBuff(AttributeBuffEffect2 parentEffect, int buffValue)
        {
            ParentEffect = parentEffect;
            BuffValue = buffValue;


            parentEffect.AttributeBuffs.Add(this);
        }
    }

}