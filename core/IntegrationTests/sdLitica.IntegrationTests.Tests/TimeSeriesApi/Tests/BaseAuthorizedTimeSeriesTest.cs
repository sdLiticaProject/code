using NUnit.Framework;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi.Models;
using sdLitica.IntegrationTests.TestUtils.RestUtils.Extensions;

namespace sdLitica.IntegrationTests.Tests.TimeSeriesApi.Tests
{
	public class BaseAuthorizedTimeSeriesTest : TimeSeriesApiTest
	{
		protected string Session;

		[SetUp]
		public void Login()
		{
			Session = ProfileFacade.PostLogin(new TestLoginModel
			{
				Email = Configuration.UserName,
				Password = Configuration.Password
			}).AssertSuccess().GetTokenFromResponse();
		}

		[TearDown]
		public void Logout()
		{
			ProfileFacade.PostLogout(Session);
		}
	}
}
