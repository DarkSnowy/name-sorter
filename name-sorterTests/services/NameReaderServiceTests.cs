using Microsoft.VisualStudio.TestTools.UnitTesting;
using name_sorter.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace name_sorterTests.services;

[TestClass]
public class NameReaderServiceTests
{
    [TestMethod]
    public async Task ReadNamesFromFileTest()
    {
        var nameReader = new NameReaderService();
        var filepath = "unsorted-unit-test-names.txt";

        if (!File.Exists(filepath))
        {
            // If for some reason the text file expected to exist is not there, then the WriteNamesToFileTest unit test recreates it each time, so execute that test
            await new NameWriterServiceTests().WriteNamesToFileTest();
        }

        var names = await nameReader.ReadNamesFromFile(filepath, CancellationToken.None);
        Assert.IsNotNull(names);
        Assert.HasCount(11, names);

        var lastNameValuePair = names.First();
        var name = $"{lastNameValuePair.Value.First().Key} {lastNameValuePair.Key}";
        Assert.AreEqual("Marin Alvarez", name);
    }
}