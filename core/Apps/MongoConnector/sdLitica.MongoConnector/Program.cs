using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using sdLitica.AnalysisResults.Repositories;
using sdLitica.Bootstrap.Extensions;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Bus;
using sdLitica.Events.Integration;
using sdLitica.Utils.Abstractions;
using sdLitica.Utils.Settings;

namespace sdLitica.MongoConnector
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var registry = host.Services.GetRequiredService<IEventRegistry>();
            registry.Register<ModuleHeartbeatEvent>(Exchanges.ModuleHeartbeats);

            AnalysisResultsListener.ListenForAddEvents(host.Services);
            AnalysisResultsListener.ListenForRequestEvents(host.Services);
            host.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
                    BsonSerializer.RegisterSerializer(new DateTimeSerializer(BsonType.String));

                    services.AddTransient<IAppSettings, AppSettings>();
                    services.AddSingleton<IMongoClient>(provider =>
                    {
                        var settings = provider.GetRequiredService<IAppSettings>().AnalysisResultsSettings;
                        return new MongoClient(settings.ConnectionString);
                    });
                    services.AddSingleton<IAnalysisResultsRepository, AnalysisResultsRepository>();

                    services.AddEventsAndMessages();
                    services.AddHostedService<ModuleHeartbeatWorker>();
                    services.AddTransient<AnalysisResultsEventHandler>();
                });
    }
}