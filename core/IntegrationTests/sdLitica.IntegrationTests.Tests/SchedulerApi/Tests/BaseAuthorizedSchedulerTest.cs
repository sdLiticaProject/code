using NUnit.Framework;
using sdLitica.IntegrationTests.TestUtils.Facades.ProfileApi.Models;
using sdLitica.IntegrationTests.TestUtils.RestUtils.Extensions;

namespace sdLitica.IntegrationTests.Tests.SchedulerApi.Tests
{
	public class BaseAuthorizedSchedulerTest : SchedulerApiTest
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
