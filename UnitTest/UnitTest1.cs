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
            data.EncryptedData = "���� � ����� ����� �������";
            data.Key = "�������";
            var expectedValue = "���� � ����� ����� �������";

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
            data.DecryptedData = "���� � ����� ����� �������";
            data.Key = "�������";
            var expectedValue = "���� � ����� ����� �������";

            //Act
            data.EncryptedData = Decryptor.Encode(data.DecryptedData, data.Key);

            //Assert
            Assert.AreEqual(expectedValue, data.EncryptedData);
        }
    }
}
