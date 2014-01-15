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
            var tordek = PrepareCharacter(game, "Tordek", Position.Create(1, 1));
            var regdar = PrepareCharacter(game, "Regdar", Position.Create(1, 2));

            var round = 1;
            while (PlayerOk(tordek) && PlayerOk(regdar))
            {
                Console.WriteLine("Round: " + round);

                DoAttack(game, tordek, regdar);
                Console.WriteLine("- Regdar: ");
                DoAttack(game, regdar, tordek);

                Console.WriteLine("- Summary: ");
                Console.WriteLine("-- Tordek: " + tordek.CharacterSheet.HitPoints + "hp");
                Console.WriteLine("-- Regdar: " + regdar.CharacterSheet.HitPoints + "hp");

                round++;
            }

            Console.WriteLine("Winner: " + (PlayerOk(tordek) ? "Tordek" : "Regdar"));
        }

        private static void DoAttack(IGame game, ICharacter attacker, ICharacter target)
        {
            if (!PlayerOk(attacker))
                return;

            Console.WriteLine(string.Format("- {0} attacks {1}: ", attacker.CharacterSheet.Name, target.CharacterSheet.Name));
            game.DiceMonitor.Clear();
            game.MeleeAttack(attacker, target);

            foreach (var roll in game.DiceMonitor.GetAllRolls())
            {
                if (roll.IsCheck)
                    Console.WriteLine(string.Format("-- {1}: {3}(1d{2}) + {4} = {5}; DC = {6} => {7}",
                                                    attacker.CharacterSheet.Name,
                                                    roll.Type,
                                                    roll.D,
                                                    roll.Roll,
                                                    roll.Bonus,
                                                    roll.Result,
                                                    roll.Check.DC,
                                                    roll.Check.Success ? "Success" : "fail"
                                          ));
                else
                    Console.WriteLine(string.Format("-- {1}: {3}(1d{2}) + {4} = {5}",
                                                    attacker.CharacterSheet.Name,
                                                    roll.Type,
                                                    roll.D,
                                                    roll.Roll,
                                                    roll.Bonus,
                                                    roll.Result
                                          ));
            }
        }

        private static ICharacter PrepareCharacter(IGame game, string name, Position position)
        {
            var character = Factory.CreateCharacter(name);

            var weapon = Factory.CreateWeapon();
            game.EquipWeapon(character, weapon);

            var armor = Factory.CreateArmor();
            game.EquipArmor(character, armor);

            game.AddCharacter(character, position);

            return character;
        }

        private static bool PlayerOk(ICharacter player)
        {
            return player.CharacterSheet.HitPoints > 0;
        }
    }
}
