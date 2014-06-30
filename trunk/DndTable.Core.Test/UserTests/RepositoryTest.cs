using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Characters;
using DndTable.Core.Entities;
using DndTable.Core.Factories;
using DndTable.Core.Persistence;
using NUnit.Framework;

namespace DndTable.Core.Test.UserTests
{
    [TestFixture]
    public class RepositoryTest
    {
        [Test]
        public void SaveAndLoad()
        {
            var repository = new Repository(@"E:\Data\Projects\DndTableOnline\Data\", @"E:\Data\Projects\DndTableOnline\Data\Characters\");

            // Save
            {
                // Board
                var entities = new List<BaseEntity>();
                entities.Add(new Wall()
                                 {
                                     Position = Position.Create(1, 2)
                                 });
                repository.SaveBoard("test", 10, 20, entities);


                // Chars
                var sheet = new CharacterSheet()
                                {
                                    Name = "hero",
                                    Race = CharacterRace.Human,
                                    Strength = 14,
                                    Dexterity = 11,
                                    Constitution = 12,
                                    Intelligence = 8,
                                    Wisdom = 9,
                                    Charisma = 10,
                                    MaxHitPoints = 20
                                };
                repository.SaveCharacterSheet("test char", sheet);
            }

            // Load
            {
                // Board
                int maxX, maxY;
                List<BaseEntity> entities;
                repository.LoadBoard("test2", out maxX, out maxY, out entities);

                // Chars
                var sheet = new CharacterSheet();
                repository.LoadCharacterSheet("test char", ref sheet);
            }
        }
    }
}
