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
        public void Save()
        {
            var repository = new Repository(@"E:\Data\Projects\DndTableOnline\Data\");
            var entities = new List<BaseEntity>();
            entities.Add(new Wall()
                             {
                                 Position = Position.Create(1, 2)
                             });
            repository.SaveBoard("test", 10, 20, entities);
        }
    }
}
