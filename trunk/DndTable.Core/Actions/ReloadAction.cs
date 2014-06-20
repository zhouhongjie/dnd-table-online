using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;

namespace DndTable.Core.Actions
{
    class ReloadAction : BaseAction
    {
        internal ReloadAction(ICharacter executer) : base(executer)
        {
        }

        public override void Do()
        {
            // Has weapon?
            if ((Executer.CharacterSheet.EquipedWeapon == null))
                throw new InvalidOperationException("attacker has no equiped weapon");

            // Needs reload?
            if (!Executer.CharacterSheet.EquipedWeapon.NeedsReload)
                throw new InvalidOperationException("attacker's equiped weapon does not require a reload");

            Register();

            // TODO: limited nr of arrows
            var reloadInfo = GetReloadInfo();
            reloadInfo.IsLoaded = true;

        }

        public override ActionTypeEnum Type
        {
            get { return GetReloadInfo().ActionType; }
        }

        public override string Description
        {
            get { return "Reload"; } // Add nr of partials to reload?
        }

        private ReloadInfo GetReloadInfo()
        {
            var weapon = Executer.CharacterSheet.EquipedWeapon as Weapon;
            if (weapon == null)
                throw new InvalidOperationException("Reload triggered without a proper weapon");

            return weapon.ReloadInfo;
        }
    }
}
