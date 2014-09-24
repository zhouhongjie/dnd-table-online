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
                        Executer.Give(PotionFactory.CreatePotionOfCatsGrace());
                    else if (choice == 2)
                        Executer.Give(PotionFactory.CreatePotionOfBullsStrength());
                    else if (choice == 3)
                        Executer.Give(WeaponFactory.HalfSpear());
                    else if (choice == 4)
                        Executer.Give(WeaponFactory.BattleAxe());
                    else if (choice == 5)
                        Executer.Give(WeaponFactory.CrossbowLight());
                    else if (choice == 6)
                        Executer.Give(WeaponFactory.Longbow());
                    else if (choice == 7)
                        Executer.Give(WeaponFactory.Rapier());
                    else 
                        Executer.Give(PotionFactory.CreatePotionOfCureLightWound());
                }
            }

            // make chest empty!
            _chest.IsUsed = true;
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
