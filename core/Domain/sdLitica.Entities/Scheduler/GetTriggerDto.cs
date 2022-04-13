using System;

namespace sdLitica.Entities.Scheduler
{
	public class GetTriggerDto
	{
		public string JobType { get; set; }
		public string TriggerKey { get; set; }
		public DateTimeOffset? NextFireTime { get; set; }
		public DateTimeOffset? LastFireTime { get; set; }
	}
}
