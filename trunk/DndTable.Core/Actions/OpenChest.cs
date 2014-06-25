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
    internal class OpenChest : BaseAction
    {
        private Chest _chest;

        internal OpenChest(ICharacter executer, Chest chest)
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
                    var potionChoice = DiceRoller.Roll(Executer, DiceRollEnum.Loot, 3, 0);
                    IPotion currentPotion = null;
                    if (potionChoice == 1)
                        currentPotion = PotionFactory.CreatePotionOfCureLightWound();
                    else if (potionChoice == 2)
                        currentPotion = PotionFactory.CreatePotionOfCatsGrace();
                    else if (potionChoice == 3)
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
