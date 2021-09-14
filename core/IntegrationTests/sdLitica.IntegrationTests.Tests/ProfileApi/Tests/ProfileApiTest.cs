using NUnit.Framework;
using sdLitica.IntegrationTests.Tests.ProfileApi.Extensions;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi;
using sdLitica.IntegrationTests.TestUtils.RestUtils;
using Serilog.Core;

namespace sdLitica.IntegrationTests.Tests.ProfileApi.Tests
{
    public class ProfileApiTest : BaseWithDefaultUserTest
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

            ProfileGivenExtension.Init(Configuration);
            ProfileWhenExtension.Init(Facade);
            ProfileThenExtension.Init(Facade);
        }
    }
}