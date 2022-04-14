using sdLitica.IntegrationTests.TestUtils.BddUtils;
using sdLitica.IntegrationTests.TestUtils.Facades.SchedulerApi.Models;

namespace sdLitica.IntegrationTests.Tests.SchedulerApi.Extensions
{
	public static class SchedulerGivenExtension
	{
		public static GivenStatement NewTrigger(this GivenStatement givenStatement,
			TestCreateNewTriggerModel trigger, string testKey = null)
		{
			givenStatement.GetStatementLogger()
				.Information("[{ContextStatement}] Saving trigger to create {Trigger}",
					givenStatement.GetType().Name, trigger);
			givenStatement.AddData(trigger, BddKeyConstants.TriggerToCreate + testKey);
			return givenStatement;
		}
	}
}
