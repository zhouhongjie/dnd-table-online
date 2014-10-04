using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;

namespace DndTable.Core.Factories
{
    internal static class ImmunityBuilder
    {
        public static void AddUndeadImmunities(CharacterImmunities immunities)
        {
            immunities.ImmuneToMindInfuencingEffects = true;
            immunities.ImmuneToPoison = true;
            immunities.ImmuneToSleep = true;
            immunities.ImmuneToParalysis = true;
            immunities.ImmuneToStunning = true;
            immunities.ImmuneToDisease = true;
            immunities.ImmuneToCriticalHits = true;
            immunities.ImmuneToSubdualDamage = true;
            immunities.ImmuneToAbilityDamage = true;
            immunities.ImmuneToEnergyDrain = true;
            immunities.ImmuneToDeathFromMassiveDamage = true;
        }
    }
}
