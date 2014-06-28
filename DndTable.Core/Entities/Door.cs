using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Factories;

namespace DndTable.Core.Entities
{
    internal class Door : BaseEntity
    {
        public override EntityTypeEnum EntityType
        {
            get { return EntityTypeEnum.Door; }
        }

        public override bool IsBlocking
        {
            get { return !IsOpen; }
        } 

        internal override List<IAction> GetUseActions(ICharacter character, AbstractActionFactory actionFactory)
        {
            return new List<IAction>()
                       {
                           actionFactory.UseDoor(character, this)
                       };
        }

        internal bool IsOpen { get; set; }
    }
}
