using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Integration;
using sdLitica.Utils.Abstractions;
using sdLitica.Utils.Settings;

namespace sdLitica.MongoConnector
{
    public class ModuleHeartbeatWorker: BackgroundService
    {
        private const string ServiceName = "MongoConnector";
        private readonly AnalyticsSettings _analyticsSettings;
        private readonly IEventBus _eventBus;
        private readonly ILogger<ModuleHeartbeatWorker> _logger;

        public ModuleHeartbeatWorker(
            IAppSettings appSettings,
            IEventBus eventBus,
            ILogger<ModuleHeartbeatWorker> logger
        )
        {
            _analyticsSettings = appSettings.AnalyticsSettings;
            _logger = logger;
            _eventBus = eventBus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("{Name} is alive at: {Time}", ServiceName, DateTimeOffset.Now);
                var @event = new ModuleHeartbeatEvent(ServiceName);
                _eventBus.Publish(@event);
                await Task.Delay(_analyticsSettings.ModuleHeartbeatInterval, stoppingToken);
            }
        }
    }
}