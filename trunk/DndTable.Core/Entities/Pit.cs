using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core.Entities
{
    internal class Pit : BaseEntity
    {
        public override EntityTypeEnum EntityType
        {
            get { return EntityTypeEnum.Pit; }
        }
    }
}
