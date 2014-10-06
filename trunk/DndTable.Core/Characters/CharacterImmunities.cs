using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core.Characters
{
    internal class CharacterImmunities : ICharacterImmunities
    {
        public bool ImmuneToMindInfuencingEffects { get; internal set; }

        public bool ImmuneToPoison { get; internal set; }

        public bool ImmuneToSleep { get; internal set; }

        public bool ImmuneToParalysis { get; internal set; }

        public bool ImmuneToStunning { get; internal set; }

        public bool ImmuneToDisease { get; internal set; }

        public bool ImmuneToCriticalHits { get; internal set; }

        public bool ImmuneToSubdualDamage { get; internal set; }

        public bool ImmuneToAbilityDamage { get; internal set; }

        public bool ImmuneToEnergyDrain { get; internal set; }

        public bool ImmuneToDeathFromMassiveDamage { get; internal set; }

        public bool ImmuneToCold { get; internal set; }

        public bool HalfDamageFromPiercing { get; internal set; }

        public bool HalfDamageFromSlashing { get; internal set; }


        // Playerhandbook p47 (sneak attacks)
        public bool ImmuneToSneakAttacks { get { return ImmuneToCriticalHits; } }
    }
}
