using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace DndTable.Core.Test
{
    [TestFixture]
    public class AttackTest
    {
        private static Game CreateGame()
        {
            var board = new Board(10, 10);
            return new Game(board, null);
        }

        private static ICharacter CreateCharacter(int ac, int meleeAttack, int meleeDamage)
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Hit()
        {
            
        }
    }
}
