using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using sdLitica.Events.Integration;
using sdLitica.Messages.Abstractions;
using sdLitica.Messages.Consumers;
using sdLitica.Utils.Abstractions;
using sdLitica.Utils.Settings;
using System;
using System.IO;

namespace MessageDaemon
{
    class Program
    {
        public static IConfigurationRoot Configuration;
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            IAppSettings appSettings = new AppSettings(configuration);
            IBrokerSettings brokerSettings = new BrokerSettings(appSettings);
            BrokerConnection brokerConnection = new BrokerConnection(brokerSettings);
            IConsumer consumer = new MessageConsumer(brokerConnection);
            consumer.Subscribe("TimeSeriesQueue", (TimeSeriesAnalysisEvent @event) =>
            {

            });
            
            Console.WriteLine("Hello World!");
        }

        static void ConfigureServices(IServiceCollection serviceCollection)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.Development.json", false)
                .Build();

            serviceCollection.AddSingleton<IConfigurationRoot>(Configuration);
        }
    }
}
