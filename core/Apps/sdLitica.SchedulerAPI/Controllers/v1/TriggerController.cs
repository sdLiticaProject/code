using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sdLitica.Entities.Scheduler;
using sdLitica.Exceptions.Http;
using sdLitica.TimeSeries.Services;
using sdLitica.Triggers.Services;

namespace sdLitica.SchedulerAPI.Controllers.v1
{
	/// <summary>
	/// Controller to control triggers for pull data updates
	/// </summary>
	[Produces("application/json")]
	[Route("api/v1/triggers")]
	[Authorize]
	public class TriggerController : BaseApiController
	{
		private readonly ITriggersService _triggerService;
		private readonly ITimeSeriesMetadataService _seriesMetadataService;

		/// <summary>
		/// Creates controller with trigger service
		/// </summary>
		/// <param name="triggerService"></param>
		/// <param name="seriesMetadataService"></param>
		public TriggerController(ITriggersService triggerService, ITimeSeriesMetadataService seriesMetadataService)
		{
			_triggerService = triggerService;
			_seriesMetadataService = seriesMetadataService;
		}

		/// <summary>
		/// Creates new trigger for the influx metadata
		/// </summary>
		/// <param name="dto">Cron schedule for the trigger</param>
		[HttpPost]
		public void AddNewTrigger([FromBody] CreateNewTriggerDto dto)
		{
			if (_seriesMetadataService.HasUserTimeSeriesMetadata(UserId, dto.MetadataId.ToString()))
			{
				_triggerService.AddNewTrigger(dto.MetadataId, dto.CronSchedule);
			}
			else
			{
				throw new NotFoundException($"Could not find metadata {dto.MetadataId} for user {UserId}");
			}
		}

		/// <summary>
		/// Changes cron schedule for the influx metadata
		/// </summary>
		/// <param name="id">Metadata id</param>
		/// <param name="dto">New cron schedule for the trigger</param>
		[HttpPost("{id}")]
		public void EditTrigger([FromRoute] Guid id, [FromBody] EditTriggerDto dto)
		{
			if (_seriesMetadataService.HasUserTimeSeriesMetadata(UserId, id.ToString()))
			{
				_triggerService.EditTrigger(id, dto.CronSchedule);
			}
			else
			{
				throw new NotFoundException($"Could not find metadata {id} for user {UserId}");
			}
		}

		/// <summary>
		/// Removes update trigger from the influx metadata
		/// </summary>
		/// <param name="id">Metadata id</param>
		[HttpDelete("{id}")]
		public void RemoveTrigger([FromRoute] Guid id)
		{
			_triggerService.RemoveTrigger(id);
		}

		/// <summary>
		/// Pauses trigger for the influx metadata
		/// </summary>
		/// <param name="id">Metadata id</param>
		[HttpPost("{id}/pause")]
		public void PauseJob([FromRoute] Guid id)
		{
			_triggerService.PauseJob(id);
		}

		/// <summary>
		/// Resumes trigger for the influx metadata
		/// </summary>
		/// <param name="id">Metadata id</param>
		[HttpPost("{id}/resume")]
		public void ResumeJob([FromRoute] Guid id)
		{
			_triggerService.ResumeJob(id);
		}

		/// <summary>
		/// Retrieves all user triggers
		/// </summary>
		[HttpGet]
		public IReadOnlyCollection<GetTriggerDto> GetAllTriggers()
		{
			var metadataList = _seriesMetadataService.GetByUserId(UserId);
			return metadataList
				.Select(e => (_triggerService.GetTrigger(e.Id), _triggerService.GetJobInfo(e.Id)))
				.Where(e => e.Item1 != null && e.Item2 != null)
				.Select(e => new GetTriggerDto()
				{
					JobType = e.Item2.JobType.Name,
					TriggerKey = e.Item2.Key.Name,
					LastFireTime = e.Item1.GetPreviousFireTimeUtc(),
					NextFireTime = e.Item1.GetNextFireTimeUtc(),
				}).ToList();
		}
	}
}
