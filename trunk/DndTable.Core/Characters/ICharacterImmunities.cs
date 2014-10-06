using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core.Characters
{
    public interface ICharacterImmunities
    {
        bool ImmuneToMindInfuencingEffects { get; }
        bool ImmuneToPoison { get; }
        bool ImmuneToSleep { get; }
        bool ImmuneToParalysis { get; }
        bool ImmuneToStunning { get; }
        bool ImmuneToDisease { get; }
        bool ImmuneToCriticalHits { get; }
        bool ImmuneToSubdualDamage { get; }
        bool ImmuneToAbilityDamage { get; }
        bool ImmuneToEnergyDrain { get; }
        bool ImmuneToDeathFromMassiveDamage { get; }
        bool ImmuneToSneakAttacks { get; }

        bool ImmuneToCold { get; }

        bool HalfDamageFromPiercing { get; }
        bool HalfDamageFromSlashing { get; }
    }
}
