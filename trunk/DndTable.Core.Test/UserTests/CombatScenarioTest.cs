﻿using System;
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
            var tordek = PrepareCharacter(game, "Tordek", Position.Create(1, 1), WeaponFactory.CrossbowLight(), ArmorFactory.Leather());
            //var regdar = PrepareCharacter(game, "Regdar", Position.Create(1, 5), WeaponFactory.CrossbowLight());
            //var tordek = PrepareCharacter(game, "Tordek", Position.Create(1, 1), WeaponFactory.Dagger());
            var regdar = PrepareCharacter(game, "Regdar", Position.Create(1, 2), WeaponFactory.Dagger(), null);

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
            {
                var attackAction = possibleActions.FirstOrDefault(a => a is IMeleeAttackAction) as IMeleeAttackAction;
                if (attackAction != null)
                {
                    var target = GetOtherCharacter(current, allCharacters);
                    Console.WriteLine(string.Format("- {0} attacks {1}: ", current.CharacterSheet.Name, target.CharacterSheet.Name));

                    attackAction.Target(target).Do();
                }
            }
            {
                var attackAction = possibleActions.FirstOrDefault(a => a is IRangeAttackAction) as IRangeAttackAction;
                if (attackAction != null)
                {
                    var target = GetOtherCharacter(current, allCharacters);
                    Console.WriteLine(string.Format("- {0} shoots {1}: ", current.CharacterSheet.Name, target.CharacterSheet.Name));

                    attackAction.Target(target).Do();
                }
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
                    Console.WriteLine(string.Format("-- {0}-{1}: {3}(1d{2}) + {4} = {5}; DC = {6} => {7}",
                                                    roll.Roller.CharacterSheet.Name,
                                                    roll.Type,
                                                    roll.D,
                                                    roll.Roll,
                                                    roll.Bonus,
                                                    roll.Result,
                                                    roll.Check.DC,
                                                    roll.Check.Success ? "Success" : "fail"
                                          ));
                else
                    Console.WriteLine(string.Format("-- {0}-{1}: {3}(1d{2}) + {4} = {5}",
                                                    roll.Roller.CharacterSheet.Name,
                                                    roll.Type,
                                                    roll.D,
                                                    roll.Roll,
                                                    roll.Bonus,
                                                    roll.Result
                                          ));
            }
        }

        private static ICharacter PrepareCharacter(IGame game, string name, Position position, IWeapon weapon, IArmor armor)
        {
            var character = Factory.CreateCharacter(name);

            game.EquipWeapon(character, weapon);
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
