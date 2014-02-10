﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core.Actions
{
    public interface IStraightLineMove : IAction
    {
        IAction Target(Position position);

        /// <summary>
        /// MaxRange in Tiles (not in feet)
        /// </summary>
        int MaxRange { get; }

        /// <summary>
        /// MaxRange in Tiles (not in feet)
        /// </summary>
        int MinRange { get; }
    }
}
