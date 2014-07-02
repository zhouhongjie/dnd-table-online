using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using Attribute = DndTable.Core.Characters.Attribute;

namespace DndTable.Core.Spells
{
    internal class AttributeBuffEffect : BaseEffect
    {
        private readonly Attribute _attributeToBuff;
        private readonly int _buffValue;

        internal AttributeBuffEffect(CharacterSheet sheet, int durationInRounds, Attribute attributeToBuff, int buffValue)
            : base(sheet, durationInRounds)
        {
            _attributeToBuff = attributeToBuff;
            _buffValue = buffValue;
        }

        internal override void Apply()
        {
            _attributeToBuff.AddBuff(_buffValue);
        }
    }
}
