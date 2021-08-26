using NUnit.Framework;
using sdLitica.IntegrationTests.ProfileApi.Tools.Models;
using sdLitica.IntegrationTests.RestApiTestBase;

namespace sdLitica.IntegrationTests.ProfileApi.Tests
{
    public class BaseAuthorizedTest : ProfileApiTest
    {
        protected string Session;

        [SetUp]
        public void Login()
        {
            Session = _facade.PostLogin(new TestLoginModel
            {
                Email = Configuration.UserName,
                Password = Configuration.Password
            }).AssertSuccess().GetTokenFromResponse();
        }
        [TearDown]
        public void Logout()
        { 
            _facade.PostLogout(Session);
        }
    }
}