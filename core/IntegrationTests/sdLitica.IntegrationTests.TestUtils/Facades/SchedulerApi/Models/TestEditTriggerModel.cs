namespace sdLitica.IntegrationTests.TestUtils.Facades.SchedulerApi.Models
{
	public class TestEditTriggerModel : TestModel
	{
		public string CronSchedule { get; set; }
		public string FetchUrl { get; set; }
	}
}
