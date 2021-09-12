using NUnit.Framework;
using sdLitica.IntegrationTests.ProfileApi.Tools;
using sdLitica.IntegrationTests.ProfileApi.Tools.Helpers;
using sdLitica.IntegrationTests.RestApiTestBase;

namespace sdLitica.IntegrationTests.ProfileApi.Tests
{
    [TestFixture]
    public class ProfileApiTest : BaseAuthenticatedApiTest
    {
        protected ProfileApiFacade Facade;

        [OneTimeSetUp]
        public void InitFacade()
        {
            // correct config
            Assert.AreEqual("user@sdcliud.io", Configuration.UserName);
            // todo bootstrap it later
            Facade = new ProfileApiFacade(
                Logger,
                Configuration.RootUrl);

            ProfileWhenExtension.Init(Logger, Configuration);
            ProfileThenExtension.Init(Logger, Configuration);
        }
    }
}