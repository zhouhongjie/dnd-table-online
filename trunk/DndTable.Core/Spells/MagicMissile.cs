using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;

namespace DndTable.Core.Spells
{
    internal class MagicMissile : BaseSpell
    {
        internal MagicMissile(ICharacter caster)
            : base(caster)
        {}

        public override string Description { get { return "Magic missile"; } }

        // Medium (100 ft. + 10 ft./level)
        public override int MaxRange { get { return 100; } }

        public override bool CastOn(Characters.ICharacter target)
        {
            throw new NotImplementedException();
        }
    }
}
