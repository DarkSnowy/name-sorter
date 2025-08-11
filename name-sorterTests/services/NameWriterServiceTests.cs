using name_sorter.services;
using System.Text;

namespace name_sorterTests.services
{
    [TestClass]
    public class NameWriterServiceTests
    {
        [TestMethod]
        public async Task WriteNamesToFileTest()
        {
            var nameWriter = new NameWriterService();
            var filepath = "unsorted-unit-test-names.txt";
            // Includes some extra white space to make sure nothing breaks because of it
            var stringBuilder = new StringBuilder(" Janet Parsons \r\n Vaughn Lewis \r\n Adonis Julius Archer \r\n Shelby Nathan Yoder \r\nMarin Alvarez\r\nLondon Lindsey\r\n" +
                "Beau Tristan Bentley\r\nLeo Gardner\r\nHunter Uriah Mathew Clarke\r\nMikayla Lopez\r\nFrankie Conner Ritter");
            
            await nameWriter.WriteNamesToFile(filepath, stringBuilder, CancellationToken.None);

            var lines = await File.ReadAllLinesAsync(filepath, CancellationToken.None);
            Assert.AreEqual(11, lines.Length);
            Assert.AreEqual(" Janet Parsons ", lines[0]);
            Assert.AreEqual("Frankie Conner Ritter", lines[10]);
        }
    }
}