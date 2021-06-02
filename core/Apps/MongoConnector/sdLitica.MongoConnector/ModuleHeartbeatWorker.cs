using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Integration;
using sdLitica.Utils.Abstractions;
using sdLitica.Utils.Settings;

namespace sdLitica.MongoConnector
{
    // TODO: probably, it can be generalized and moved to core library
    public class ModuleHeartbeatWorker: BackgroundService
    {
        private const string ServiceName = "MongoConnector";
        private readonly AnalyticsSettings _analyticsSettings;
        private readonly IServiceProvider _services;
        private readonly ILogger<ModuleHeartbeatWorker> _logger;

        public ModuleHeartbeatWorker(
            IAppSettings appSettings,
            IServiceProvider services,
            ILogger<ModuleHeartbeatWorker> logger
        )
        {
            _analyticsSettings = appSettings.AnalyticsSettings;
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("{Name} is alive at: {Time}", ServiceName, DateTimeOffset.Now);
                var @event = new ModuleHeartbeatEvent(ServiceName);
                using var scope = _services.CreateScope();
                var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
                eventBus.Publish(@event);
                await Task.Delay(_analyticsSettings.ModuleHeartbeatInterval, stoppingToken);
            }
        }
    }
}