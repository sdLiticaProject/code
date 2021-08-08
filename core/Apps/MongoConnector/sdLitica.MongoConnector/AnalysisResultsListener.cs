using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Bus;
using sdLitica.Events.Integration;
using sdLitica.Events.Rpc;

namespace sdLitica.MongoConnector
{
    public static class AnalysisResultsListener
    {
        public static void ListenForAddEvents(IServiceProvider services)
        {
            var registry = services.GetEventRegistry();
            registry.Register<NewAnalysisResultEvent>(Exchanges.AnalysisResults);
            var handler = services.GetRequiredService<AnalysisResultsEventHandler>();
            services.WithEventBus(eventBus => { eventBus.Subscribe((NewAnalysisResultEvent @event) => { handler.HandleNewAnalysisResult(@event); }); });
        }

        public static void ListenForRequestEvents(IServiceProvider services)
        {
            var registry = services.GetEventRegistry();
            registry.Register<AnalysisResultRequestEvent>(Exchanges.AnalysisResults);
            registry.Register<AnalysisResultResponseEvent>(Exchanges.AnalysisResults);
            var handler = services.GetRequiredService<AnalysisResultsEventHandler>();
            var handlers = new Dictionary<Type, Func<IEvent, IEvent>>
            {
                {typeof(AnalysisResultRequestEvent), handler.HandleAnalysisResultRequest}
            };
            RpcServerRabbitMq.RegisterHandlers(services, handlers);
        }

        private static IEventRegistry GetEventRegistry(this IServiceProvider services) =>
            services.GetRequiredService<IEventRegistry>();

        private static void WithEventBus(this IServiceProvider services, Action<IEventBus> action)
        {
            using var scope = services.CreateScope();
            var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
            action(eventBus);
        }
    }
}