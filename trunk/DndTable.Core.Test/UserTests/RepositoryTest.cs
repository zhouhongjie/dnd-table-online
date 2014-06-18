using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Entities;
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
            var repository = new Repository(@"E:\Data\Projects\DndTableOnline\Data\");

            // Save
            {
                var entities = new List<BaseEntity>();
                entities.Add(new Wall()
                                 {
                                     Position = Position.Create(1, 2)
                                 });
                repository.SaveBoard("test", 10, 20, entities);
            }

            // Load
            {
                int maxX, maxY;
                List<BaseEntity> entities;
                repository.LoadBoard("test2", out maxX, out maxY, out entities);
            }
        }
    }
}
