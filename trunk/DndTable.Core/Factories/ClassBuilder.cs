using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;

namespace DndTable.Core.Factories
{
    public class ClassBuilder
    {
        private IDiceRoller _diceRoller;

        internal ClassBuilder(IDiceRoller diceRoller)
        {
            _diceRoller = diceRoller;
        }

        public void AddBarbarianLevel(ICharacter character)
        {
            throw new NotImplementedException();
        }

        public void AddBardLevel(ICharacter character)
        {
            throw new NotImplementedException();
        }

        public void AddClericLevel(ICharacter character)
        {
            throw new NotImplementedException();
        }

        public void AddWDruidLevel(ICharacter character)
        {
            throw new NotImplementedException();
        }

        public void AddFighterLevel(ICharacter character)
        {
            throw new NotImplementedException();
        }

        public void AddMonkLevel(ICharacter character)
        {
            throw new NotImplementedException();
        }

        public void AddPaladinLevel(ICharacter character)
        {
            throw new NotImplementedException();
        }

        public void AddRangerLevel(ICharacter character)
        {
            throw new NotImplementedException();
        }

        public void AddRogueLevel(ICharacter character)
        {
            var sheet = CharacterSheet.GetEditableSheet(character);

            if (sheet.Level.Keys.Count(_ => _ != CharacterClass.Rogue) > 0)
                throw new NotImplementedException("TODO: multiclass");

            if (!sheet.Level.ContainsKey(CharacterClass.Rogue))
            {
                // Lvl 1
                sheet.CanSneakAttack = true;

                //var extraHitpoints = _diceRoller.Roll(character, DiceRollEnum.Hitpoints, 6, 0);
                var starterHitpoints = 6;
                sheet.HpProperty.BaseValue = starterHitpoints;
                sheet.MaxHpProperty.BaseValue = starterHitpoints;

                sheet.BaseAttackBonus += 0;
                sheet.FortitudeProperty.BaseValue += 0;
                sheet.ReflexProperty.BaseValue += 2;
                sheet.WillProperty.BaseValue += 0;

                sheet.Level.Add(CharacterClass.Rogue, 1);
                return;
            }

            throw new NotImplementedException();
        }

        public void AddSorcererLevel(ICharacter character)
        {
            throw new NotImplementedException();
        }

        public void AddWizardLevel(ICharacter character)
        {
            throw new NotImplementedException();
        }
    }
}
