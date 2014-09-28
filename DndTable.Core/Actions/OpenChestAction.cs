using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Entities;
using DndTable.Core.Factories;
using DndTable.Core.Items;
using DndTable.Core.Weapons;

namespace DndTable.Core.Actions
{
    internal class OpenChestAction : BaseAction
    {
        private Chest _chest;

        internal OpenChestAction(ICharacter executer, Chest chest)
            : base(executer)
        {
            _chest = chest;
        }

        public override void Do()
        {
            using (var context = Calculator.CreateActionContext(this))
            {
                _Do(context);
            }
        }

        private void _Do(Calculator.CalculatorActionContext context)
        {
            Register();

            HandleAttackOfOpportunity(context);
            if (!Executer.CharacterSheet.CanAct())
                return;

            if (_chest.IsUsed)
                return;

            // TODO: depend on chest properties (ex. chest quality?) or .. prefill chest manually?
            {
                var nrOfItems = DiceRoller.Roll(Executer, DiceRollEnum.Loot, 3, 0);

                for (var i = 0; i < nrOfItems; i++)
                {
                    var choice = DiceRoller.Roll(Executer, DiceRollEnum.Loot, 10, 0);

                    if (choice == 1)
                        Loot(context, PotionFactory.CreatePotionOfCatsGrace());
                    else if (choice == 2)
                        Loot(context, PotionFactory.CreatePotionOfBullsStrength());
                    else if (choice == 3)
                        Loot(context, WeaponFactory.HalfSpear());
                    else if (choice == 4)
                        Loot(context, WeaponFactory.BattleAxe());
                    else if (choice == 5)
                        Loot(context, WeaponFactory.CrossbowLight());
                    else if (choice == 6)
                        Loot(context, WeaponFactory.Longbow());
                    else if (choice == 7)
                        Loot(context, WeaponFactory.Rapier());
                    else
                        Loot(context, PotionFactory.CreatePotionOfCureLightWound());
                }
            }

            // make chest empty!
            _chest.IsUsed = true;
        }

        private void Loot(Calculator.CalculatorActionContext context, IItem item)
        {
            Executer.Give(item);
            context.Message(string.Format("{0} loots {1}", Executer.CharacterSheet.Name, item.Description));

        }

        public override ActionTypeEnum Type
        {
            get { return ActionTypeEnum.FullRound; }
        }

        public override ActionCategoryEnum Category { get { return ActionCategoryEnum.Context; } }

        public override string Description
        {
            get { return "Open chest"; }
        }

        public override bool RequiresUI
        {
            get { return false; }
        }
    }
}
