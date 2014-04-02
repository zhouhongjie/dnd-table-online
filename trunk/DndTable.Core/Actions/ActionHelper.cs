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
            // No AoO when unarmed, or with range weapon
            if (threatener.CharacterSheet.EquipedWeapon == null || threatener.CharacterSheet.EquipedWeapon.IsRanged)
                return false;

            // TODO: reach weapons, etc ...
            return MathHelper.GetTilesDistance(victim.Position, threatener.Position) == 1;
        }

        public static bool IsFlanking(ICharacter attacker, ICharacter target, List<ICharacter> participants)
        {
            foreach (var participant in participants)
            {
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
