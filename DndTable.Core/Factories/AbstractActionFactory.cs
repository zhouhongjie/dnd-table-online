using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Entities;
using DndTable.Core.Items;
using DndTable.Core.Spells;
using DndTable.Core.Weapons;

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

        internal IAttackAction MeleeAttack(ICharacter attacker)
        {
            var action = new AttackAction(attacker);
            action.Initialize(this);
            return action;
        }

        internal IAttackAction RangeAttack(ICharacter attacker)
        {
            return MeleeAttack(attacker);
        }

        internal IAttackAction NaturalAttack(ICharacter attacker)
        {
            return MeleeAttack(attacker);
        }

        internal IMoveAction Move(ICharacter character)
        {
            var action = new MoveAction(character);
            action.Initialize(this);
            return action;
        }

        internal IStraightLineMove FiveFootStep(ICharacter character)
        {
            var action = new FiveFootStep(character);
            action.Initialize(this);
            return action;
        }

        internal IAttackAction Charge(ICharacter character)
        {
            var action = new ChargeAction(character);
            action.Initialize(this);
            return action;
        }

        internal IAttackAction PartialCharge(ICharacter character)
        {
            var action = new PartialChargeAction(character);
            action.Initialize(this);
            return action;
        }

        internal IAction Reload(ICharacter character)
        {
            var action = new ReloadAction(character);
            action.Initialize(this);
            return action;
        }

        internal IAction DrinkPotion(ICharacter character, IPotion potion)
        {
            var action = new UsePotionAction(character, potion);
            action.Initialize(this);
            return action;
        }

        internal IAction OpenChest(ICharacter character, Chest chest)
        {
            var action = new OpenChestAction(character, chest);
            action.Initialize(this);
            return action;
        }

        internal IAction UseDoor(ICharacter character, Door door)
        {
            var action = new UseDoorAction(character, door);
            action.Initialize(this);
            return action;
        }

        public IAction SwitchWeapon(ICharacter character, IWeapon weapon)
        {
            var action = new SwitchWeaponAction(character, weapon);
            action.Initialize(this);
            return action;
        }

        public IAction CastSpell(ICharacter character, ISpell spell)
        {
            var action = new CastSpellAction(character, spell);
            action.Initialize(this);
            return action;
        }

        public IAction GiveItem(ICharacter character, IItem item, ICharacter receiver)
        {
            var action = new GiveItemAction(character, item).Target(receiver) as BaseAction;
            action.Initialize(this);
            return action;
        }

        public IAction ApplyPotion(ICharacter character, IPotion item, ICharacter receiver)
        {
            var action = new ApplyPotionAction(character, item).Target(receiver) as BaseAction;
            action.Initialize(this);
            return action;
        }
    }
}
