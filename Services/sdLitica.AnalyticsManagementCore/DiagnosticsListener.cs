using Microsoft.Extensions.DependencyInjection;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Bus;
using sdLitica.Events.Integration;
using sdLitica.Relational.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.AnalyticsManagementCore
{
    public static class DiagnosticsListener
    {

        static IEventRegistry _eventRegistry;
        static IEventBus _eventBus;
        static OperationRepository _operationRepository;
        public static IServiceProvider _services;


        public static void Initialize(IEventRegistry eventRegistry, IEventBus eventBus, OperationRepository operationRepository)//, IServiceProvider services)
        {
            _eventRegistry = eventRegistry;
            _eventBus = eventBus;
            _operationRepository = operationRepository;
        }

        public static void Listen()
        {
            _eventRegistry.Register<DiagnosticsEvent>(Exchanges.Diagnostics);
            _eventBus.Subscribe((DiagnosticsEvent @event) =>
            {
                using (var scope = _services.CreateScope())
                {
                    _operationRepository = scope.ServiceProvider.GetRequiredService<OperationRepository>();
                    _operationRepository.SetStatus(@event.Operation, @event.Operation.Status);
                }
            });
        }
    }
}
