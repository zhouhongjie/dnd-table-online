using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Items;

namespace DndTable.Core.Factories
{
    public class AbstractActionFactory
    {
        internal Encounter Encounter { get; private set; }
        internal Board Board  { get; private set; }
        internal IDiceRoller DiceRoller { get; private set; }

        internal AbstractActionFactory(Encounter encounter, Board board, IDiceRoller diceRoller)
        {
            Encounter = encounter;
            Board = board;
            DiceRoller = diceRoller;
        }

        public IAttackAction MeleeAttack(ICharacter attacker)
        {
            var action = new AttackAction(attacker);
            action.Initialize(this);
            return action;
        }

        public IAttackAction RangeAttack(ICharacter attacker)
        {
            return MeleeAttack(attacker);
        }

        public IMoveAction Move(ICharacter character)
        {
            var action = new MoveAction(character);
            action.Initialize(this);
            return action;
        }

        public IStraightLineMove FiveFootStep(ICharacter character)
        {
            var action = new FiveFootStep(character);
            action.Initialize(this);
            return action;
        }

        public IAttackAction Charge(ICharacter character)
        {
            var action = new ChargeAction(character);
            action.Initialize(this);
            return action;
        }

        public IAction Reload(ICharacter character)
        {
            var action = new ReloadAction(character);
            action.Initialize(this);
            return action;
        }

        public IAction DrinkPotion(ICharacter character, IPotion potion)
        {
            var action = new UsePotionAction(character, potion);
            action.Initialize(this);
            return action;
        }
    }
}
