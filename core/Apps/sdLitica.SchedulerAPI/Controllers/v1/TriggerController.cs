using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quartz;
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
			if (!_seriesMetadataService.HasUserTimeSeriesMetadata(UserId, dto.MetadataId.ToString()))
			{
				throw new NotFoundException($"Could not find metadata {dto.MetadataId} for user {UserId}");
			}
			if (!CronExpression.IsValidExpression(dto.CronSchedule))
			{
				throw new InvalidRequestException(
					$"Could not edit trigger {dto.MetadataId} with cron schedule {dto.CronSchedule}");
			}
			if (Uri.TryCreate(dto.FetchUrl, UriKind.Absolute, out var uriResult) &&
			    (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
			{
				_triggerService.AddNewTrigger(dto.MetadataId, dto.CronSchedule, dto.FetchUrl);
			}
			else
			{
				throw new InvalidRequestException($"Could not create uri from {dto.FetchUrl}");
			}
		}

		/// <summary>
		/// Changes cron schedule for the influx metadata
		/// </summary>
		/// <param name="id">Metadata id</param>
		/// <param name="dto">New cron schedule for the trigger</param>
		[HttpPut("{id}")]
		public void EditTrigger([FromRoute] Guid id, [FromBody] EditTriggerDto dto)
		{
			if (!_seriesMetadataService.HasUserTimeSeriesMetadata(UserId, id.ToString()))
			{
				throw new NotFoundException($"Could not find metadata {id} for user {UserId}");
			}
			if (!CronExpression.IsValidExpression(dto.CronSchedule))
			{
				throw new InvalidRequestException(
					$"Could not edit trigger {id} with cron schedule {dto.CronSchedule}");
			}
			if (Uri.TryCreate(dto.FetchUrl, UriKind.Absolute, out var uriResult) &&
			    (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
			{
				_triggerService.EditTrigger(id, dto.CronSchedule, dto.FetchUrl);
			}
			else
			{
				throw new InvalidRequestException($"Could not create uri from {dto.FetchUrl}");
			}
		}

		/// <summary>
		/// Removes update trigger from the influx metadata
		/// </summary>
		/// <param name="id">Metadata id</param>
		[HttpDelete("{id}")]
		public void RemoveTrigger([FromRoute] Guid id)
		{
			if (_seriesMetadataService.HasUserTimeSeriesMetadata(UserId, id.ToString()))
			{
				_triggerService.RemoveTrigger(id);
			}
			else
			{
				throw new NotFoundException($"Could not find metadata {id} for user {UserId}");
			}
		}

		/// <summary>
		/// Pauses trigger for the influx metadata
		/// </summary>
		/// <param name="id">Metadata id</param>
		[HttpPost("{id}/pause")]
		public void PauseJob([FromRoute] Guid id)
		{
			if (_seriesMetadataService.HasUserTimeSeriesMetadata(UserId, id.ToString()))
			{
				_triggerService.PauseJob(id);
			}
			else
			{
				throw new NotFoundException($"Could not find metadata {id} for user {UserId}");
			}
		}

		/// <summary>
		/// Resumes trigger for the influx metadata
		/// </summary>
		/// <param name="id">Metadata id</param>
		[HttpPost("{id}/resume")]
		public void ResumeJob([FromRoute] Guid id)
		{
			if (_seriesMetadataService.HasUserTimeSeriesMetadata(UserId, id.ToString()))
			{
				_triggerService.ResumeJob(id);
			}
			else
			{
				throw new NotFoundException($"Could not find metadata {id} for user {UserId}");
			}
		}

		/// <summary>
		/// Retrieves all user triggers
		/// </summary>
		[HttpGet]
		public IReadOnlyCollection<GetTriggerDto> GetAllTriggers()
		{
			var metadataList = _seriesMetadataService.GetByUserId(UserId);
			return metadataList
				.Select(e => (Trigger: _triggerService.GetTrigger(e.Id), JobInfo: _triggerService.GetJobInfo(e.Id),
					JobResult: e.LastJobResult))
				.Where(e => e.Trigger != null && e.JobInfo != null)
				.Select(e => new GetTriggerDto()
				{
					JobType = e.JobInfo.JobType.Name,
					TriggerKey = e.JobInfo.Key.Name,
					LastFireTime = e.Trigger.GetPreviousFireTimeUtc(),
					NextFireTime = e.Trigger.GetNextFireTimeUtc(),
					LastJobResult = e.JobResult,
				}).ToList();
		}
	}
}
