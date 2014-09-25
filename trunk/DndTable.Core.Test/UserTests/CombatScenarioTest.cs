using System;
using System.Collections.Generic;
using DndTable.Core.Actions;
using DndTable.Core.Armors;
using DndTable.Core.Characters;
using DndTable.Core.Dice;
using DndTable.Core.Factories;
using DndTable.Core.Log;
using DndTable.Core.Test.Helpers;
using DndTable.Core.Weapons;
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
            var tordek = EncounterHelper.PrepareCharacter(game, "Tordek", Position.Create(1, 1), WeaponFactory.CrossbowLight(), ArmorFactory.Leather());
            //var regdar = EncounterHelper.PrepareCharacter(game, "Regdar", Position.Create(1, 5), WeaponFactory.CrossbowLight());
            //var tordek = EncounterHelper.PrepareCharacter(game, "Tordek", Position.Create(1, 1), WeaponFactory.Dagger());
            var regdar = EncounterHelper.PrepareCharacter(game, "Regdar", Position.Create(1, 2), WeaponFactory.Dagger(), null);

            Console.WriteLine("Start encounter");

            game.Logger.Clear();
            game.DiceMonitor.Clear();
            var encounter = game.StartEncounter(new List<ICharacter>() { tordek, regdar });
            //DiceMonitorToConsole(game);
            LoggerToConsole(game);

            var currentPlayer = encounter.GetCurrentCharacter();
            while (currentPlayer != null)
            {
                Console.WriteLine("Round {0} => player: {1}", encounter.GetRound(), currentPlayer.CharacterSheet.Name);

                var possibleActions = encounter.GetPossibleActionsForCurrentCharacter();

                game.Logger.Clear();
                game.DiceMonitor.Clear();
                EncounterHelper.AttackOtherWhenPossible(possibleActions, currentPlayer, game.GetCharacters());
                //DiceMonitorToConsole(game);
                LoggerToConsole(game);

                SummaryToConsole(tordek, regdar);

                currentPlayer = encounter.GetNextCharacter();

                // Temp end condition
                if (!PlayerOk(tordek) || !PlayerOk(regdar))
                    break;
            }

            Console.WriteLine("Winner: " + (PlayerOk(tordek) ? "Tordek" : "Regdar"));
        }

        private static void SummaryToConsole(ICharacter tordek, ICharacter regdar)
        {
            Console.WriteLine("- Summary: ");
            Console.WriteLine("-- Tordek: " + tordek.CharacterSheet.HitPoints + "hp");
            Console.WriteLine("-- Regdar: " + regdar.CharacterSheet.HitPoints + "hp");
        }

        public static void DiceMonitorToConsole(IGame game)
        {
            foreach (var roll in game.DiceMonitor.GetAllRolls())
            {
                Console.WriteLine(roll.Description);
            }
        }

        public static void LoggerToConsole(IGame game)
        {
            foreach (var message in game.Logger.GetAllMessages())
            {
                Console.WriteLine(message);
            }
        }

        private static bool PlayerOk(ICharacter player)
        {
            return player.CharacterSheet.HitPoints > 0;
        }
    }
}
