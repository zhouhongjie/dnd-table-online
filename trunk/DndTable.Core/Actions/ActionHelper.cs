using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;

namespace DndTable.Core.Actions
{
    static class ActionHelper
    {
        public static bool IsInThreatenedArea(ICharacter victim, ICharacter threatener)
        {
            var sheet = CharacterSheet.GetEditableSheet(threatener);

            if (sheet.HasNaturalWeapons)
            {
                // No AoO with ranged weapons
                if (sheet.NaturalWeapons[0].IsRanged)
                    return false;
            }
            else
            {
                // No AoO when unarmed, or with range weapon
                if (threatener.CharacterSheet.EquipedWeapon == null || threatener.CharacterSheet.EquipedWeapon.IsRanged)
                    return false;
            }

            // TODO: reach weapons, etc ...
            return MathHelper.GetTilesDistance(victim.Position, threatener.Position) == 1;
        }

        public static bool IsFlanking(ICharacter attacker, ICharacter target, List<ICharacter> participants)
        {
            // http://www.enworld.org/forum/showthread.php?149852-Can-you-flank-with-a-ranged-weapon
            // logic: you can only flank if you are threatening the target + there is an ally on the opposite side threatening the target
            if (!ActionHelper.IsInThreatenedArea(target, attacker))
                return false;

            foreach (var participant in participants)
            {
                // No flanking with dead guys!
                if (!participant.CharacterSheet.CanAct())
                    continue;

                // only flanking with allies
                if (participant.CharacterSheet.FactionId != attacker.CharacterSheet.FactionId)
                    continue;

                if (ActionHelper.IsInThreatenedArea(target, participant))
                {
                    var attackerDX = Math.Sign(attacker.Position.X - target.Position.X);
                    var attackerDY = Math.Sign(attacker.Position.Y - target.Position.Y);

                    var participantDX = Math.Sign(participant.Position.X - target.Position.X);
                    var participantDY = Math.Sign(participant.Position.Y - target.Position.Y);

                    if ((attackerDX == participantDX * -1) &&
                        (attackerDY == participantDY * -1))
                        return true;
                }
            }

            return false;
        }

    }
}
