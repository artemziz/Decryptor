using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab3.Models;
using Lab3.Controllers;


namespace UnitTest
{
    [TestClass]
    public class DecryptorTests
    {
        [TestMethod]
        public void DecryptFromText()
        {

            //Arrange
            var data = new CryptData();
            data.EncryptedData = "ץכנü ב ן‏ךüû הרץעצ צמבםנ‏¸";
            data.Key = "ךכאנםוע";
            var expectedValue = "ךאנכ ף ךכאנû ףךנאכ ךמנאככû";

            //Act
            data.DecryptedData = Decryptor.Decode(data.EncryptedData, data.Key);

            //Assert
            Assert.AreEqual(expectedValue, data.DecryptedData);
        }

        [TestMethod]
        public void EncryptFromText()
        {
           
            //Arrange
            var data = new CryptData();
            data.DecryptedData = "ךאנכ ף ךכאנû ףךנאכ ךמנאככû";
            data.Key = "ךכאנםוע";
            var expectedValue = "ץכנü ב ן‏ךüû הרץעצ צמבםנ‏¸";

            //Act
            data.EncryptedData = Decryptor.Encode(data.DecryptedData, data.Key);

            //Assert
            Assert.AreEqual(expectedValue, data.EncryptedData);
        }

       
    }
}
