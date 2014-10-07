using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core.Characters
{
    internal class HitpointsProperty
    {
        internal HitpointsProperty(string description, Attribute conAttribute, Dictionary<CharacterClass, int> level)
        {
            Description = description;
            _linkedConAttribute = conAttribute;
            _level = level;
        }

        public string Description { get; private set; }

        private Attribute _linkedConAttribute;
        private Dictionary<CharacterClass, int> _level;

        // Determined by DiceRoller (+ init hp)
        public int BaseValue { get; internal set; }

        internal int GetValue()
        {
            // TODO: buffs, feats, ....
            return BaseValue + _linkedConAttribute.GetAbilityBonus() * GetTotalNrOfLevels();
        }

        private int GetTotalNrOfLevels()
        {
            int total = 0;
            _level.Values.ToList().ForEach(_ => total += _);
            return total;
        }
    }
}
