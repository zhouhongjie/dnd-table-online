﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DndTable.Core
{
    public interface ICharacter : IEntity
    {
        ICharacterSheet CharacterSheet { get; }
    }
}