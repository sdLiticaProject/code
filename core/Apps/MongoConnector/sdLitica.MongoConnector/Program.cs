using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using sdLitica.Bootstrap.Extensions;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Bus;
using sdLitica.Events.Integration;
using sdLitica.Utils.Abstractions;
using sdLitica.Utils.Settings;

namespace sdLitica.MongoConnector
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var registry = host.Services.GetRequiredService<IEventRegistry>();
            registry.Register<ModuleHeartbeatEvent>(Exchanges.ModuleHeartbeats);

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services.AddTransient<IAppSettings, AppSettings>();
                    services.AddEventsAndMessages();
                    services.AddHostedService<ModuleHeartbeatWorker>();
                });
    }
}