using System;
using NUnit.Framework;

namespace DndTable.Core.Test.UserTests
{
    [TestFixture]
    public class CombatScenarioTest
    {
        [Test]
        public void SimpleCombat()
        {
            var game = Factory.CreateGame(10, 10);
            var tordek = PrepareCharacter(game, Position.Create(1, 1));
            var regdar = PrepareCharacter(game, Position.Create(1, 2));

            var round = 1;
            while (PlayerOk(tordek) && PlayerOk(regdar))
            {
                Console.WriteLine("Round: " + round);

                if (PlayerOk(tordek))
                {
                    game.MeleeAttack(tordek, regdar);
                    Console.WriteLine("- Tordek attack: 1d" + game.DiceMonitor.GetLastRoll().D + " = " + game.DiceMonitor.GetLastRoll().Roll);
                }
                if (PlayerOk(regdar))
                {
                    game.MeleeAttack(regdar, tordek);
                    Console.WriteLine("- Regdar attack: 1d" + game.DiceMonitor.GetLastRoll().D + " = " + game.DiceMonitor.GetLastRoll().Roll);
                }

                Console.WriteLine("- Tordek: " + tordek.CharacterSheet.HitPoints);
                Console.WriteLine("- Regdar: " + regdar.CharacterSheet.HitPoints);

                round++;
            }

            Console.WriteLine("Winner: " + (PlayerOk(tordek) ? "Tordek" : "Regdar"));
        }

        private static ICharacter PrepareCharacter(IGame game, Position position)
        {
            var character = Factory.CreateCharacter();
            var weapon = Factory.CreateWeapon();

            game.EquipWeapon(character, weapon);
            game.AddCharacter(character, position);

            return character;
        }

        private static bool PlayerOk(ICharacter player)
        {
            return player.CharacterSheet.HitPoints > 0;
        }
    }
}
