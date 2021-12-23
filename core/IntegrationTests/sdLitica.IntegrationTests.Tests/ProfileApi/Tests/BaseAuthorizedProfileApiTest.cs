using NUnit.Framework;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi.Models;
using sdLitica.IntegrationTests.TestUtils.RestUtils.Extensions;

namespace sdLitica.IntegrationTests.Tests.ProfileApi.Tests
{
    public class BaseAuthorizedProfileApiTest : ProfileApiTest
    {
        protected string Session;

        [SetUp]
        public void Login()
        {
            Session = Facade.PostLogin(new TestLoginModel
            {
                Email = Configuration.UserName,
                Password = Configuration.Password
            }).AssertSuccess().GetTokenFromResponse();
        }
        [TearDown]
        public void Logout()
        { 
            Facade.PostLogout(Session);
        }
    }
}