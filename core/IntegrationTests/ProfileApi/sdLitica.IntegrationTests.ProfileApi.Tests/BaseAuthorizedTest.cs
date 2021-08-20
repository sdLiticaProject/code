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
            var response = _facade.PostLogin(new TestLoginModel
            {
                Email = Configuration.UserName,
                Password = Configuration.Password
            }).AssertSuccess();
            Session = _facade.GetTokenFromResponse(response);
        }
        [TearDown]
        public void Logout()
        { 
            _facade.PostLogout(Session);
        }
    }
}