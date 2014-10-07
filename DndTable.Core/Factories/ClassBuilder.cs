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
            const int hitDie = 8;

            var sheet = CharacterSheet.GetEditableSheet(character);

            if (sheet.Level.Keys.Count(_ => _ != CharacterClass.Cleric) > 0)
                throw new NotImplementedException("TODO: multiclass");

            // Lvl 1
            if (!sheet.Level.ContainsKey(CharacterClass.Cleric))
            {
                InitHp(sheet, hitDie);
                SetStats(sheet, 0, 2, 0, 2);

                // TODO: Max Spells

                sheet.Level.Add(CharacterClass.Cleric, 1);
                return;
            }
            // Lvl 2
            if (sheet.Level[CharacterClass.Cleric] == 1)
            {
                AddExtraHp(character, hitDie);
                SetStats(sheet, 1, 3, 0, 3);

                // TODO: Max Spells

                sheet.Level[CharacterClass.Cleric]++;
                return;
            }

            throw new NotImplementedException();
        }

        public void AddDruidLevel(ICharacter character)
        {
            throw new NotImplementedException();
        }

        public void AddFighterLevel(ICharacter character)
        {
            const int hitDie = 10;

            var sheet = CharacterSheet.GetEditableSheet(character);

            if (sheet.Level.Keys.Count(_ => _ != CharacterClass.Fighter) > 0)
                throw new NotImplementedException("TODO: multiclass");

            // Lvl 1
            if (!sheet.Level.ContainsKey(CharacterClass.Fighter))
            {
                // TODO: bonus feat

                InitHp(sheet, hitDie);
                SetStats(sheet, 1, 2, 0, 0);

                sheet.Level.Add(CharacterClass.Fighter, 1);
                return;
            }
            // Lvl 2
            if (sheet.Level[CharacterClass.Fighter] == 1)
            {
                // TODO: bonus feat

                AddExtraHp(character, hitDie);
                SetStats(sheet, 2, 3, 0, 0);

                sheet.Level[CharacterClass.Fighter]++;
                return;
            }

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
            const int hitDie = 6;

            var sheet = CharacterSheet.GetEditableSheet(character);

            if (sheet.Level.Keys.Count(_ => _ != CharacterClass.Rogue) > 0)
                throw new NotImplementedException("TODO: multiclass");

            // Lvl 1
            if (!sheet.Level.ContainsKey(CharacterClass.Rogue))
            {
                sheet.CanSneakAttack = true;

                InitHp(sheet, hitDie);
                SetStats(sheet, 0, 0, 2, 0);

                sheet.Level.Add(CharacterClass.Rogue, 1);
                return;
            }
            // Lvl 2
            if (sheet.Level[CharacterClass.Rogue] == 1)
            {
                // TODO: evasion feat

                AddExtraHp(character, hitDie);
                SetStats(sheet, 1, 0, 3, 0);

                sheet.Level[CharacterClass.Rogue]++;
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
            const int hitDie = 4;

            var sheet = CharacterSheet.GetEditableSheet(character);

            if (sheet.Level.Keys.Count(_ => _ != CharacterClass.Wizard) > 0)
                throw new NotImplementedException("TODO: multiclass");

            // Lvl 1
            if (!sheet.Level.ContainsKey(CharacterClass.Wizard))
            {
                InitHp(sheet, hitDie);
                SetStats(sheet, 0, 0, 0, 2);

                // TODO: Max Spells

                sheet.Level.Add(CharacterClass.Wizard, 1);
                return;
            }
            // Lvl 2
            if (sheet.Level[CharacterClass.Wizard] == 1)
            {
                AddExtraHp(character, hitDie);
                SetStats(sheet, 1, 0, 0, 3);

                // TODO: Max Spells

                sheet.Level[CharacterClass.Wizard]++;
                return;
            }

            throw new NotImplementedException();
        }

        private void InitHp(CharacterSheet sheet, int hitDie)
        {
            int starterHitpoints = hitDie;
            sheet.HpProperty.BaseValue = starterHitpoints;
            sheet.MaxHpProperty.BaseValue = starterHitpoints;
        }

        private void AddExtraHp(ICharacter character, int hitDie)
        {
            var sheet = CharacterSheet.GetEditableSheet(character);
            var extraHitpoints = _diceRoller.Roll(character, DiceRollEnum.Hitpoints, hitDie, 0);
            sheet.HpProperty.BaseValue += extraHitpoints;
            sheet.MaxHpProperty.BaseValue += extraHitpoints;
        }

        private void SetStats(CharacterSheet sheet, int bab, int fort, int reflex, int will)
        {
            sheet.BaseAttackBonus += bab;
            sheet.FortitudeProperty.BaseValue += fort;
            sheet.ReflexProperty.BaseValue += reflex;
            sheet.WillProperty.BaseValue += will;
        }
    }
}
