using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using sdLitica.IntegrationTests.ProfileApi.Tools;
using sdLitica.IntegrationTests.RestApiTestBase;

namespace sdLitica.IntegrationTests.ProfileApi.Tests
{
    [TestFixture]
    public class ProfileApiTest : BaseAuthenticatedApiTest
    {
        protected ProfileApiFacade _facade;

        [OneTimeSetUp]
        public void InitFacade()
        {
            // correct config
            Assert.AreEqual("user@sdcliud.io", Configuration.UserName);
            // todo bootstrap it later
            _facade = new ProfileApiFacade(
                Logger,
                Configuration.RootUrl);
        }
    }
}