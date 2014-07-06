using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Entities;
using DndTable.Core.Factories;
using DndTable.Core.Items;

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
                var nrOfPotions = DiceRoller.Roll(Executer, DiceRollEnum.Loot, 3, 0);

                for (var i = 0; i < nrOfPotions; i++)
                {
                    var potionChoice = DiceRoller.Roll(Executer, DiceRollEnum.Loot, 10, 0);
                    IPotion currentPotion = null;

                    // 1/10
                    if (potionChoice == 1)
                        currentPotion = PotionFactory.CreatePotionOfCatsGrace();
                    // 1/10
                    else if (potionChoice == 2)
                        currentPotion = PotionFactory.CreatePotionOfBullsStrength();
                    // 8/10
                    else 
                        currentPotion = PotionFactory.CreatePotionOfBullsStrength();

                    CharacterSheet.GetEditableSheet(Executer).Potions.Add(currentPotion);
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
