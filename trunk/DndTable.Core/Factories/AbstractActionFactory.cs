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
        private readonly Encounter _encounter;
        private readonly Board _board;
        private readonly IDiceRoller _diceRoller;

        internal AbstractActionFactory(Encounter encounter, Board board, IDiceRoller diceRoller)
        {
            _encounter = encounter;
            _board = board;
            _diceRoller = diceRoller;
        }

        public IAttackAction MeleeAttack(ICharacter attacker)
        {
            var action = new MeleeAttackAction(attacker);
            action.Initialize(_diceRoller, _encounter, _board);
            return action;
        }

        public IAttackAction RangeAttack(ICharacter attacker)
        {
            return MeleeAttack(attacker);
        }

        public IMoveAction Move(ICharacter character)
        {
            var action = new MoveAction(character);
            action.Initialize(_diceRoller, _encounter, _board);
            return action;
        }
    }
}
