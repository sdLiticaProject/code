using System;
using NUnit.Framework;
using sdLitica.Test.BaseApiTest;
namespace sdLitica.Test.RestApiTests
{
    [TestFixture]
    public class UnitTest1: BaseApiTest.BaseApiTest
    {
        [Test]
        public void Test1()
        {
            Assert.AreEqual("Ivan", configuration.UserName);
        }
    }
}
