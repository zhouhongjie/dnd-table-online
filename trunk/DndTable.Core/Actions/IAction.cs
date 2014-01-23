﻿using DndTable.Core.Characters;

namespace DndTable.Core.Actions
{
    public enum ActionTypeEnum
    {
        Partial, MoveEquivalent,
    }

    public interface IAction
    {
        void Do();

        ActionTypeEnum Type { get; }
    }
}
