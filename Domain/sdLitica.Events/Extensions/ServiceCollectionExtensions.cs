using Microsoft.Extensions.DependencyInjection;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Bus;
using sdLitica.Messages.Abstractions;
using sdLitica.Messages.Consumers;
using sdLitica.Messages.Producers;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Events.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEvents(this IServiceCollection serviceCollection)
        {
            // Registries must have a singleton
            serviceCollection.AddSingleton<IEventRegistry, EventRegistry>();
            
            serviceCollection.AddSingleton<BrokerConnection>();
            serviceCollection.AddSingleton<IBrokerManager, BrokerManager>();
            serviceCollection.AddTransient<IBrokerSettings, BrokerSettings>();
            
            serviceCollection.AddScoped<IPublisher, MessagePublisher>();
            serviceCollection.AddScoped<IConsumer, MessageConsumer>();
            serviceCollection.AddScoped<IEventBus, EventBus>();            
            
        }
    }
}
