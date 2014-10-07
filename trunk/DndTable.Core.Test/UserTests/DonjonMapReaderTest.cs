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
    public class DonjonMapReaderTest
    {
        [Test]
        public void Test()
        {
            var reader = new DonjonMapReader();
            reader.SetDonjonFolder(@"E:\Data\Projects\DndTableOnline\Data\Donjon\");

            int maxX, maxY;
            List<BaseEntity> entities;

            var name = "The Sepulcher of Adamant Terror 01 (tsv)";

            Assert.That(reader.LoadBoard(name, out maxX, out maxY, out entities));

            // Save to xml
            var repository = new Repository(@"E:\Data\Projects\DndTableOnline\Data\Maps\", @"E:\Data\Projects\DndTableOnline\Data\Characters\");
            repository.SaveBoard("test2", maxX, maxY, entities);
        }
    }
}
