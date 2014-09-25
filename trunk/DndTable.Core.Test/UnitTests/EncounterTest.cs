using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Factories;
using DndTable.Core.Test.Helpers;
using DndTable.Core.Test.UserTests;
using DndTable.Core.Weapons;
using NUnit.Framework;

namespace DndTable.Core.Test.UnitTests
{
    [TestFixture]
    public class EncounterTest
    {
        private List<ICharacter> _allCharacters;
        private IEncounter _encounter;
        private IGame _game;

        [SetUp]
        public void Setup()
        {
            _game = Factory.CreateGame(10, 10);
            var char1 = EncounterHelper.PrepareCharacter(_game, "char1", Position.Create(1, 1), new Weapon() { DamageD = 4 }, null);
            var char2 = EncounterHelper.PrepareCharacter(_game, "char2", Position.Create(1, 2), new Weapon() { DamageD = 4 }, null);

            _allCharacters = new List<ICharacter>() {char1, char2};
            _encounter = _game.StartEncounter(_allCharacters);   
        }

        [Test]
        public void Max2MoveEquivalent()
        {
            AssertDoMove();
            AssertDoMove();

            AssertActionNotPossible(ActionTypeEnum.MoveEquivalent);
        }

        [Test]
        public void Max1StandardAction()
        {
            AssertDoAttack();

            AssertActionNotPossible(ActionTypeEnum.Standard);
        }

        [Test]
        public void StandardActionPlusMoveEquivalent()
        {
            AssertDoMove();
            AssertDoAttack();

            AssertActionNotPossible(ActionTypeEnum.MoveEquivalent);
            AssertActionNotPossible(ActionTypeEnum.Standard);
        }

        [Test]
        public void StandardActionPlusMoveEquivalent2()
        {
            AssertDoAttack();
            AssertDoMove();

            AssertActionNotPossible(ActionTypeEnum.MoveEquivalent);
            AssertActionNotPossible(ActionTypeEnum.Standard);
        }

        [Test]
        public void FiveFootOnlyWhenNotMoved()
        {
            AssertActionPossible(ActionTypeEnum.FiveFootStep);

            AssertDoMove();

            AssertActionNotPossible(ActionTypeEnum.FiveFootStep);
        }

        [Test]
        public void FiveFootOnStandardActions()
        {
            AssertActionPossible(ActionTypeEnum.FiveFootStep);
            AssertDoAttack();
            AssertActionPossible(ActionTypeEnum.FiveFootStep);
        }

        [Test]
        public void ReloadEnabled()
        {
            var current = _encounter.GetCurrentCharacter();

            // No reload needed on weapon
            {
                Assert.IsFalse(current.CharacterSheet.EquipedWeapon.NeedsReload);
                AssertActionPossible<AttackAction2>();
                AssertActionNotPossible<ReloadAction>();
            }

            var reloadInfo = new ReloadInfo()
                                 {
                                     IsLoaded = true
                                 };
            (current.CharacterSheet.EquipedWeapon as Weapon).ReloadInfo = reloadInfo;

            // Load enabled weapon is loaded
            {
                Assert.IsFalse(current.CharacterSheet.EquipedWeapon.NeedsReload);
                AssertActionNotPossible<ReloadAction>();
            }
        }

        [Test]
        public void ReloadMoveEquivalent()
        {
            var current = _encounter.GetCurrentCharacter();
            var reloadInfo = new ReloadInfo()
                                 {
                                     IsLoaded = false,
                                     ActionType = ActionTypeEnum.MoveEquivalent
                                 };
            (current.CharacterSheet.EquipedWeapon as Weapon).ReloadInfo = reloadInfo;

            // Test reload 1 partial
            {
                AssertActionNotPossible<AttackAction2>();
                AssertActionPossible<ReloadAction>();

                AssertDoReload();

                AssertActionPossible<AttackAction2>();
                AssertActionNotPossible<ReloadAction>();
            }
        }

        [Test]
        public void ReloadFullRound()
        {
            var current = _encounter.GetCurrentCharacter();
            var reloadInfo = new ReloadInfo()
                                 {
                                     IsLoaded = false,
                                     ActionType = ActionTypeEnum.FullRound
                                 };
            (current.CharacterSheet.EquipedWeapon as Weapon).ReloadInfo = reloadInfo;

            // Test reload 2 partials
            {
                AssertActionNotPossible<AttackAction2>();
                AssertActionPossible<ReloadAction>();

                AssertDoReload();

                AssertActionNotPossible<AttackAction2>(); // no more partial actions left
                AssertActionNotPossible<ReloadAction>();
            }
        }

        [Test]
        public void Charge()
        {
            var current = _encounter.GetCurrentCharacter();

            // Cannot charge with ranged weapon
            current.EquipWeapon(new Weapon() { IsRanged = true });
            AssertActionNotPossible<ChargeAction>();
            // Cannot charge unarmed (YET) => unarmed combat not yet supported
            current.EquipWeapon(null);
            AssertActionNotPossible<ChargeAction>();
            // Can charge with melee weapon
            current.EquipWeapon(new Weapon() { IsRanged = false });
            AssertActionPossible<ChargeAction>();

            // Charge is FullRound action with move
            AssertDoMove();
            AssertActionNotPossible<ChargeAction>();
        }

        private void AssertActionPossible(ActionTypeEnum actionType)
        {
            var possibleActions = _encounter.GetPossibleActionsForCurrentCharacter();
            Assert.NotNull(possibleActions.FirstOrDefault(a => a.Type == actionType));
        }

        private void AssertActionPossible<T>() where T : BaseAction
        {
            var possibleActions = _encounter.GetPossibleActionsForCurrentCharacter();
            Assert.NotNull(possibleActions.FirstOrDefault(a => a is T));
        }

        private void AssertActionNotPossible(ActionTypeEnum actionType)
        {
            var possibleActions = _encounter.GetPossibleActionsForCurrentCharacter();
            Assert.Null(possibleActions.FirstOrDefault(a => a.Type == actionType));
        }

        private void AssertActionNotPossible<T>() where T : BaseAction
        {
            var possibleActions = _encounter.GetPossibleActionsForCurrentCharacter();
            Assert.Null(possibleActions.FirstOrDefault(a => a is T));
        }

        private void AssertDoAttack()
        {
            var current = _encounter.GetCurrentCharacter();
            var possibleActions = _encounter.GetPossibleActionsForCurrentCharacter();
            var attack = possibleActions.FirstOrDefault(a => a is IAttackAction) as IAttackAction;
            Assert.NotNull(attack);
            attack.Target(EncounterHelper.GetOtherCharacter(current, _allCharacters)).Do();
        }

        private void AssertDoReload()
        {
            var possibleActions = _encounter.GetPossibleActionsForCurrentCharacter();
            var action = possibleActions.FirstOrDefault(a => a is ReloadAction) as ReloadAction;
            Assert.NotNull(action);
            action.Do();
        }

        private void AssertDoMove()
        {
            var current = _encounter.GetCurrentCharacter();
            var possibleActions = _encounter.GetPossibleActionsForCurrentCharacter();
            var move = possibleActions.FirstOrDefault(a => a is IMoveAction) as IMoveAction;
            Assert.NotNull(move);
            move.DoOneStep(Position.Create(current.Position.X+1, 1));
            move.Do();
        }
    }
}
