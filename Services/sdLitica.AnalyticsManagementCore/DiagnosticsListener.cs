﻿using Microsoft.Extensions.DependencyInjection;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Bus;
using sdLitica.Events.Integration;
using sdLitica.Relational.Repositories;
using System;

namespace sdLitica.AnalyticsManagementCore
{
    /// <summary>
    /// Listener for diagnostics info of analytical operations (status, errors etc.)
    /// </summary>
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

        /// <summary>
        /// Subscribe to diagnostics info. Accesses repository to update metadata (status, errors etc.) of operations. 
        /// </summary>
        public static void Listen()
        {
            _eventRegistry.Register<DiagnosticsEvent>(Exchanges.Diagnostics);
            _eventBus.Subscribe((DiagnosticsEvent @event) =>
            {
                using (var scope = _services.CreateScope())
                {
                    _operationRepository = scope.ServiceProvider.GetRequiredService<OperationRepository>();
                    _operationRepository.Update(@event.Operation);
                    _operationRepository.SaveChanges();
                }
            });
        }
    }
}
