using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Factories;

namespace DndTable.Core.Entities
{
    internal class Chest : BaseEntity
    {
        public override EntityTypeEnum EntityType
        {
            get { return EntityTypeEnum.Chest; }
        }

        internal override List<IAction> GetUseActions(ICharacter character, AbstractActionFactory actionFactory)
        {
            return new List<IAction>()
                       {
                           actionFactory.OpenChest(character, this)
                       };
        }

    }
}
