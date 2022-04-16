using System;

namespace sdLitica.IntegrationTests.TestUtils.Facades.SchedulerApi.Models
{
	public class TestGetTriggerModel : TestModel
	{
		public string JobType { get; set; }
		public string TriggerKey { get; set; }
		public DateTimeOffset? NextFireTime { get; set; }
		public DateTimeOffset? LastFireTime { get; set; }
		public string LastJobResult { get; set; }
	}
}
