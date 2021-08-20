using NUnit.Framework;
using sdLitica.Test.BaseApiTest;

namespace RestApiTests
{
    [TestFixture]
    public class UnitTest1: BaseApiTest
    {
        [Test]
        public void EnsureTestConfigurationIsCorrect()
        {
            Assert.AreEqual("user@sdcliud.io", Configuration.UserName);
        }
    }
}
