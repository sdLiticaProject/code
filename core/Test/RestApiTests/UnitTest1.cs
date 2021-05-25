using System;
using NUnit.Framework;
using sdLitica.Test.BaseApiTest;
namespace sdLitica.Test.RestApiTests
{
    [TestFixture]
    public class UnitTest1: BaseApiTest.BaseApiTest
    {
        [Test]
        public void EnsureTestConfigurationIsCorrect()
        {
            Assert.AreEqual("user@sdcliud.io", configuration.UserName);
        }
    }
}
