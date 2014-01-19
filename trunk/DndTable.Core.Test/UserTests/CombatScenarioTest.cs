using System;
using System.Collections.Generic;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Factories;
using NUnit.Framework;
using System.Linq;

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

            Console.WriteLine("Start encounter");
            game.DiceMonitor.Clear();
            var encounter = game.StartEncounter(new List<ICharacter>() { tordek, regdar });
            DiceMonitorToConsole(game);

            var currentPlayer = encounter.GetCurrentCharacter();
            while (currentPlayer != null)
            {
                Console.WriteLine("Round {0} => player: {1}", encounter.GetRound(), currentPlayer.CharacterSheet.Name);

                var possibleActions = encounter.GetPossibleActionsForCurrentCharacter();

                game.DiceMonitor.Clear();
                AttackOtherWhenPossible(possibleActions, currentPlayer, game.GetCharacters());
                DiceMonitorToConsole(game);

                SummaryToConsole(tordek, regdar);

                currentPlayer = encounter.GetNextCharacter();

                // Temp end condition
                if (!PlayerOk(tordek) || !PlayerOk(regdar))
                    break;
            }

            Console.WriteLine("Winner: " + (PlayerOk(tordek) ? "Tordek" : "Regdar"));
        }

        private static void AttackOtherWhenPossible(List<IAction> possibleActions, ICharacter current, List<ICharacter> allCharacters)
        {
            var attackAction = possibleActions.FirstOrDefault(a => a is IMeleeAttackAction) as IMeleeAttackAction;
            if (attackAction != null)
            {
                var target = GetOtherCharacter(current, allCharacters);
                Console.WriteLine(string.Format("- {0} attacks {1}: ", current.CharacterSheet.Name, target.CharacterSheet.Name));

                attackAction.Target(target).Do();
            }
        }

        private static ICharacter GetOtherCharacter(ICharacter current, List<ICharacter> all)
        {
            foreach (var character in all)
            {
                if (character != current)
                    return character;
            }
            return null;
        }

        private static void SummaryToConsole(ICharacter tordek, ICharacter regdar)
        {
            Console.WriteLine("- Summary: ");
            Console.WriteLine("-- Tordek: " + tordek.CharacterSheet.HitPoints + "hp");
            Console.WriteLine("-- Regdar: " + regdar.CharacterSheet.HitPoints + "hp");
        }

        private static void DiceMonitorToConsole(IGame game)
        {
            foreach (var roll in game.DiceMonitor.GetAllRolls())
            {
                if (roll.IsCheck)
                    Console.WriteLine(string.Format("-- {0}: {2}(1d{1}) + {3} = {4}; DC = {5} => {6}",
                                                    roll.Type,
                                                    roll.D,
                                                    roll.Roll,
                                                    roll.Bonus,
                                                    roll.Result,
                                                    roll.Check.DC,
                                                    roll.Check.Success ? "Success" : "fail"
                                          ));
                else
                    Console.WriteLine(string.Format("-- {0}: {2}(1d{1}) + {3} = {4}",
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

            var weapon = WeaponFactory.Dagger();
            game.EquipWeapon(character, weapon);

            var armor = ArmorFactory.Leather();
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
