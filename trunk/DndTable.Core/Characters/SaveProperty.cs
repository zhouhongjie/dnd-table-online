using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core.Characters
{
    internal class SaveProperty
    {
        internal SaveProperty(string description, Attribute relatedAttribute)
        {
            Description = description;
            RelatedAttribute = relatedAttribute;
        }

        public string Description { get; private set; }
        public Attribute RelatedAttribute { get; private set; }

        public int BaseValue { get; internal set; }

        internal int GetValue()
        {
            // TODO: buffs, feats, ....
            return BaseValue + RelatedAttribute.GetAbilityBonus();
        }
    }
}
