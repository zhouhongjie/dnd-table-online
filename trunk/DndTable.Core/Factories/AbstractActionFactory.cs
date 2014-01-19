using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Dice;

namespace DndTable.Core.Factories
{
    public class AbstractActionFactory
    {
        private readonly Board _board;
        private readonly IDiceRoller _diceRoller;

        internal AbstractActionFactory(Board board, IDiceRoller diceRoller)
        {
            _board = board;
            _diceRoller = diceRoller;
        }

        public IMeleeAttackAction MeleeAttack(ICharacter attacker)
        {
            var action = new MeleeAttackAction(_diceRoller, attacker);
            return action;
        }

        public IMoveAction Move(ICharacter character)
        {
            var action = new MoveAction(_board, character);
            return action;
        }
    }
}
