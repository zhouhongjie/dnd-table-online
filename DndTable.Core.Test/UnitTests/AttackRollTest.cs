using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Dice;
using NUnit.Framework;

namespace DndTable.Core.Test.UnitTests
{
    [TestFixture]
    public class AttackRollTest
    {
        [TestCase(0, 9, false)]
        [TestCase(0, 10, true)]
        [TestCase(1, 9, true)]
        public void SuccessTest(int bonus, int roll, bool shouldHit)
        {
            var attackRoll = new AttackRoll(null, DiceRollEnum.Attack, bonus, roll, 10, 20);
            Assert.AreEqual(shouldHit, attackRoll.Success);
        }

        [TestCase(0)]
        [TestCase(10)]
        [TestCase(20)]
        [TestCase(100)]
        public void AutoSuccessTest(int ac)
        {
            var attackRoll = new AttackRoll(null, DiceRollEnum.Attack, 0, 20, 10, ac);
            Assert.That(attackRoll.Success);
        }

        [TestCase(0)]
        [TestCase(10)]
        [TestCase(20)]
        [TestCase(100)]
        public void AutoFailTest(int ac)
        {
            var attackRoll = new AttackRoll(null, DiceRollEnum.Attack, 0, 1, 10, ac);
            Assert.That(!attackRoll.Success);
        }

        [TestCase(19, 20, false)]
        [TestCase(19, 19, true)]
        [TestCase(20, 20, true)]
        [TestCase(20, 19, true)]
        public void ThreatTest(int roll, int threatRange, bool shouldThreat)
        {
            var attackRoll = new AttackRoll(null, DiceRollEnum.Attack, 5, roll, 10, threatRange);
            Assert.AreEqual(shouldThreat, attackRoll.IsThreat);
        }
    }
}
