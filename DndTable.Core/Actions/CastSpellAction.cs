﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Spells;

namespace DndTable.Core.Actions
{
    internal class CastSpellAction : BaseAction, ICastSpellAction
    {
        private BaseSpell _spell;

        internal CastSpellAction(ICharacter caster, ISpell spell)
            : base(caster)
        {
            _spell = spell as BaseSpell;
        }

        public override string Description
        {
            get { return "Cast: " + _spell.Description; }
        }

        // TODO: depends on spell
        public override ActionTypeEnum Type
        {
            get { return ActionTypeEnum.Standard; }
        }

        public override ActionCategoryEnum Category
        {
            get { return ActionCategoryEnum.Spell; }
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
            if (_spell == null)
                throw new InvalidOperationException("Spell expected");

            // TODO: depends on spell
            if (_targetCharacter == null)
                throw new InvalidOperationException("Character target expected");

            Register();

            // TODO: cast on the defence
            var hitpointsBeforeAoO = Executer.CharacterSheet.HitPoints;
            HandleAttackOfOpportunity(context);
            if (!Executer.CharacterSheet.CanAct())
                return;

            // Concentration check
            var damageByAoO = hitpointsBeforeAoO - Executer.CharacterSheet.HitPoints;
            if (damageByAoO > 0)
            {
                // TODO: concentration skill in character sheet
                if (!DiceRoller.Check(Executer, DiceRollEnum.Concentration, 20, 0, damageByAoO + 10 + _spell.Level))
                    return;
            }

            // TODO: arcane spell failure

            if (!_spell.CastOn(_targetCharacter, DiceRoller))
                return;

            // Remove spell from spell list
            // TODO: changes between wizard, sorcerer & divine spell behaviours
            CharacterSheet.GetEditableSheet(Executer).Spells.Remove(_spell);
        }

        #region ICastSpellAction Members


        public int MaxRange
        {
            get { return _spell.MaxRange; }
        }

        public ISpell Spell
        {
            get { return _spell; }
        }

        #endregion
    }
}
