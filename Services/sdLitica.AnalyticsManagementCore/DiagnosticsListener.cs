using System;
using Microsoft.Extensions.DependencyInjection;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Bus;
using sdLitica.Events.Integration;
using sdLitica.Relational.Repositories;

namespace sdLitica.AnalyticsManagementCore
{
    /// <summary>
    /// Listener for diagnostics info of analytical operations (status, errors etc.)
    /// </summary>
    public static class DiagnosticsListener
    {

        static IEventRegistry _eventRegistry;
        static IEventBus _eventBus;
        static AnalyticsOperationRequestRepository _OperationRequestRepository;
        static AnalyticsRegistry _analyticsRegistry;

        public static IServiceProvider Services { get; set; }


        public static void Initialize(IEventRegistry eventRegistry, IEventBus eventBus, AnalyticsOperationRequestRepository OperationRequestRepository, AnalyticsRegistry analyticsRegistry)//, IServiceProvider services)
        {
            _eventRegistry = eventRegistry;
            _eventBus = eventBus;
            _OperationRequestRepository = OperationRequestRepository;
            _analyticsRegistry = analyticsRegistry;
        }

        /// <summary>
        /// Subscribe to diagnostics info. Accesses repository to update metadata (status, errors etc.) of operations. 
        /// </summary>
        public static void Listen()
        {
            _eventRegistry.Register<DiagnosticsResponseEvent>(Exchanges.Diagnostics);
            _eventBus.Subscribe((DiagnosticsResponseEvent @event) =>
            {
                using (IServiceScope scope = Services.CreateScope())
                {
                    System.Console.WriteLine("GOT THE ANSWER");
                    _OperationRequestRepository = scope.ServiceProvider.GetRequiredService<AnalyticsOperationRequestRepository>();
                    _OperationRequestRepository.Update(@event.Operation);
                    _OperationRequestRepository.SaveChanges();
                }
            });
        }

        /// <summary>
        /// Subscribe to new analytical modules. It's not the best place for this method.
        /// </summary>
        public static void ListenNewModules()
        {
            _eventRegistry.Register<AnalyticModuleRegistrationRequestEvent>(Exchanges.ModuleRegistration);
            System.Console.WriteLine("HUIHUIHUIHUIHUIHUIHUI");
            _eventBus.SubscribeToTopic((AnalyticModuleRegistrationRequestEvent @event) =>
            {
                using (IServiceScope scope = Services.CreateScope())
                {
                    _analyticsRegistry = scope.ServiceProvider.GetRequiredService<AnalyticsRegistry>();
                    _analyticsRegistry.Register(@event.Module);
                }
            });
        }
    }
}
