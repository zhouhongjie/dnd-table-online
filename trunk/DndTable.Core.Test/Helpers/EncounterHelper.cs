using System;
using System.Collections.Generic;
using System.Linq;
using DndTable.Core.Actions;
using DndTable.Core.Armors;
using DndTable.Core.Characters;
using DndTable.Core.Factories;
using DndTable.Core.Weapons;

namespace DndTable.Core.Test.Helpers
{
    public static class EncounterHelper
    {
        public static void AttackOtherWhenPossible(List<IAction> possibleActions, ICharacter current, List<ICharacter> allCharacters)
        {
            {
                var attackAction = possibleActions.FirstOrDefault(a => a is IAttackAction) as IAttackAction;
                if (attackAction != null)
                {
                    var target = GetOtherCharacter(current, allCharacters);
                    Console.WriteLine(string.Format("- {0} attacks {1}: ", current.CharacterSheet.Name, target.CharacterSheet.Name));

                    attackAction.Target(target).Do();
                }
            }
        }

        public static ICharacter GetOtherCharacter(ICharacter current, List<ICharacter> all)
        {
            foreach (var character in all)
            {
                if (character != current)
                    return character;
            }
            return null;
        }

        public static ICharacter PrepareCharacter(IGame game, string name, Position position, IWeapon weapon, IArmor armor)
        {
            var character = Factory.CreateCharacter(name);

            character.EquipWeapon(weapon);
            character.EquipArmor(armor);

            game.AddCharacter(character, position);

            return character;
        }
    }
}
