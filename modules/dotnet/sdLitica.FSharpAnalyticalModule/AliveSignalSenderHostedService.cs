using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Integration;
using sdLitica.Utils.Abstractions;
using sdLitica.Utils.Models;
using sdLitica.Utils.Settings;

namespace sdLitica.FSharpAnalyticalModule
{
    public class AliveSignalSenderHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private Timer _timer;
        private IServiceProvider _services;
        private AnalyticsSettings _analyticsSettings;


        private AnalyticsModuleRegistrationModel _moduleModel = new AnalyticsModuleRegistrationModel()
        {
            ModuleGuid = Guid.NewGuid(),
            Operations = new List<AnalyticsOperationModel>
                {
                    new AnalyticsOperationModel { Name = "Mean", Description = "Description of mean", RoutingKey="fsharp1.mean" },
                    new AnalyticsOperationModel { Name = "Max", Description = "Description of max", RoutingKey="fsharp1.max" },
                    new AnalyticsOperationModel { Name = "Min", Description = "Description of min", RoutingKey="fsharp1.min" }
                }
        };

        public AliveSignalSenderHostedService(IAppSettings appSettings, IServiceProvider services)
        {
            _services = services;
            _analyticsSettings = appSettings.AnalyticsSettings;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            AnalyticsSettings analyticsSettings = _services.GetRequiredService<IAppSettings>().AnalyticsSettings;
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMilliseconds(5000));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            using (IServiceScope scope = _services.CreateScope())
            {
                IEventBus eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
                eventBus.PublishToTopic(new AnalyticModuleRegistrationRequestEvent(_moduleModel));
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

    }
}
