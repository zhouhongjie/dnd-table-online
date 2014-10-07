using System.Collections.Generic;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Dice;

namespace DndTable.Core.Weapons
{
    internal class Weapon : BaseWeapon
    {
        public override bool NeedsReload { get { return this.ReloadInfo != null && !this.ReloadInfo.IsLoaded; } }
        public ReloadInfo ReloadInfo { get; internal set; }

        internal Weapon()
        {
            NrOfDamageDice = 1;
            CriticalMultiplier = 1;
            Proficiency = WeaponProficiencyEnum.Simple;
            DamageTypes = new List<WeaponDamageTypeEnum>();
        }

        public override void Use()
        {
            if (ReloadInfo != null)
            {
                ReloadInfo.IsLoaded = false;
            }
        }

        internal  override void ApplyEffect(ICharacter target, IDiceRoller diceRoller)
        {
            // TODO
        }

    }

    internal class ReloadInfo
    {
        public bool IsLoaded { get; internal set; }
        public ActionTypeEnum ActionType { get; internal set; }
    }
}
