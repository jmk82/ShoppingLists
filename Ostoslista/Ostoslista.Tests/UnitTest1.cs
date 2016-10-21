using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ostoslista.Utils;

namespace Ostoslista.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestTimeZoneConversionDst()
        {
            var utcTime = new DateTime(2016, 10, 21, 21, 20, 0);
            
            var convertedTime = TimeConverter.ConvertToEetTime(utcTime);
            var expectedTime = new DateTime(2016, 10, 22, 0, 20, 0);

            Assert.AreEqual(expectedTime, convertedTime);
        }

        [TestMethod]
        public void TestTimeZoneConversionNormalTime()
        {
            var utcTime = new DateTime(2016, 11, 21, 21, 20, 0);

            var convertedTime = TimeConverter.ConvertToEetTime(utcTime);
            var expectedTime = new DateTime(2016, 11, 21, 23, 20, 0);

            Assert.AreEqual(expectedTime, convertedTime);
        }
    }
}
