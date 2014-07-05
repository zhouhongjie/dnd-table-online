using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Factories;
using DndTable.Core.Weapons;
using NUnit.Framework;

namespace DndTable.Core.Test.UnitTests
{
    [TestFixture]
    public class FlankingTest
    {
        [Test]
        public void NoParticipants()
        {
            var target = CreateCharacter(1, Position.Create(5, 5));
            var attacker = CreateCharacter(2, Position.Create(4, 5));
            var participants = new List<ICharacter>();

            Assert.IsFalse(ActionHelper.IsFlanking(attacker, target, participants));
        }

        [TestCase(4, 4, false)]
        [TestCase(4, 5, false)]
        [TestCase(4, 6, false)]
        [TestCase(5, 4, false)]
        [TestCase(5, 6, false)]
        [TestCase(6, 4, false)]
        [TestCase(6, 5, true)]
        [TestCase(6, 6, false)]
        public void OneParticipant1(int participantPositionX, int participantPositionY, bool expectedFlank)
        {
            var target = CreateCharacter(1, Position.Create(5, 5));
            var attacker = CreateCharacter(2, Position.Create(4, 5));
            var participants = new List<ICharacter>()
                                   {
                                       CreateCharacter(2, Position.Create(participantPositionX, participantPositionY))
                                   };

            Assert.AreEqual(expectedFlank, ActionHelper.IsFlanking(attacker, target, participants));
        }

        [TestCase(4, 4, false)]
        [TestCase(4, 5, false)]
        [TestCase(4, 6, false)]
        [TestCase(5, 4, false)]
        [TestCase(5, 6, false)]
        [TestCase(6, 4, false)]
        [TestCase(6, 5, false)]
        [TestCase(6, 6, true)]
        public void OneParticipant2(int participantPositionX, int participantPositionY, bool expectedFlank)
        {
            var target = CreateCharacter(1, Position.Create(5, 5));
            var attacker = CreateCharacter(2, Position.Create(4, 4));
            var participants = new List<ICharacter>()
                                   {
                                       CreateCharacter(2, Position.Create(participantPositionX, participantPositionY))
                                   };

            Assert.AreEqual(expectedFlank, ActionHelper.IsFlanking(attacker, target, participants));
        }

        [Test]
        public void ParticipantOutOfRange()
        {
            var target = CreateCharacter(1, Position.Create(5, 5));
            var attacker = CreateCharacter(2, Position.Create(4, 4));
            var participants = new List<ICharacter>()
                                   {
                                       CreateCharacter(2, Position.Create(7, 7))
                                   };

            Assert.IsFalse(ActionHelper.IsFlanking(attacker, target, participants));
        }

        [Test]
        public void ParticipantAlly()
        {
            var target = CreateCharacter(1, Position.Create(5, 5));
            var attacker = CreateCharacter(2, Position.Create(4, 4));
            var participants = new List<ICharacter>()
                                   {
                                       CreateCharacter(1, Position.Create(6, 6))
                                   };

            Assert.IsFalse(ActionHelper.IsFlanking(attacker, target, participants));
        }

        [Test]
        public void WithNaturalWeapon()
        {
            var target = CreateCharacter(1, Position.Create(5, 5));
            var attacker = CreateCharacter(2, Position.Create(4, 4));

            // Participant with natural weapon
            var participant = CreateCharacter(2, Position.Create(6, 6), null);
            CharacterSheet.GetEditableSheet(participant).NaturalWeapons.Add(new NaturalWeapon("dummy", true, 1, 1, 1, 1));

            var participants = new List<ICharacter>()
                                   {
                                       participant
                                   };

            Assert.IsTrue(ActionHelper.IsFlanking(attacker, target, participants));
        }

        [Test]
        public void CheckAttackerWeapon()
        {
            var target = CreateCharacter(1, Position.Create(5, 5));
            var participants = new List<ICharacter>()
                                   {
                                       CreateCharacter(2, Position.Create(6, 6))
                                   };

            // Melee => ok
            {
                var attacker = CreateCharacter(2, Position.Create(4, 4));
                Assert.IsTrue(ActionHelper.IsFlanking(attacker, target, participants));
            }
            // Natural melee => ok
            {
                var attacker = CreateCharacter(2, Position.Create(4, 4), null);
                CharacterSheet.GetEditableSheet(attacker).NaturalWeapons.Add(new NaturalWeapon("melee", true, 1, 1, 1, 1));
                Assert.IsTrue(ActionHelper.IsFlanking(attacker, target, participants));
            }
            // Unarmed => not ok
            {
                var attacker = CreateCharacter(2, Position.Create(4, 4), null);
                Assert.IsFalse(ActionHelper.IsFlanking(attacker, target, participants));
            }
            // Natural ranged => not ok
            {
                var attacker = CreateCharacter(2, Position.Create(4, 4), null);
                CharacterSheet.GetEditableSheet(attacker).NaturalWeapons.Add(new NaturalWeapon("melee", false, 1, 1, 1, 1));
                Assert.IsFalse(ActionHelper.IsFlanking(attacker, target, participants));
            }
            // Ranged => not ok
            {
                var attacker = CreateCharacter(2, Position.Create(4, 4), new Weapon() { IsRanged = true });
                Assert.IsFalse(ActionHelper.IsFlanking(attacker, target, participants));
            }
        }

        [Test]
        public void ParticipantNotThreatening()
        {
            var target = CreateCharacter(1, Position.Create(5, 5));
            var attacker = CreateCharacter(2, Position.Create(4, 4));

            // Unarmed
            {
                var participants = new List<ICharacter>()
                                       {
                                           CreateCharacter(1, Position.Create(6, 6), null)
                                       };
                Assert.IsFalse(ActionHelper.IsFlanking(attacker, target, participants));
            }

            // Ranged
            {
                var participants = new List<ICharacter>()
                                       {
                                           CreateCharacter(1, Position.Create(6, 6), new Weapon() { IsRanged = true })
                                       };
                Assert.IsFalse(ActionHelper.IsFlanking(attacker, target, participants));
            }

            // Dead
            {
                var dead = CreateCharacter(1, Position.Create(6, 6));
                CharacterSheet.GetEditableSheet(dead).ApplyDamage(100);
                var participants = new List<ICharacter>()
                                       {
                                           dead
                                       };
                Assert.IsFalse(ActionHelper.IsFlanking(attacker, target, participants));
            }
        }

        private static ICharacter CreateCharacter(int factionId, Position position)
        {
            return CreateCharacter(factionId, position, new Weapon() {IsRanged = false});
        }

        private static ICharacter CreateCharacter(int factionId, Position position, IWeapon weapon)
        {
            var sheet = new CharacterSheet();

            sheet.FactionId = factionId;

            sheet.Strength = 10;
            sheet.Dexterity = 10;
            sheet.Constitution = 10;
            sheet.Intelligence = 10;
            sheet.Wisdom = 10;
            sheet.Charisma = 10;

            sheet.HitPoints = 10;
            sheet.Speed = 30;

            sheet.EquipedWeapon = weapon;

            return new Character(sheet) {Position = position};
        }
    }
}
