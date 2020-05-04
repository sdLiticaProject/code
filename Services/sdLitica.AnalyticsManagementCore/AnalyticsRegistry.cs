using sdLitica.Events.Abstractions;
using sdLitica.Events.Bus;
using sdLitica.Events.Integration;
using sdLitica.Utils.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.AnalyticsManagementCore
{
    public class AnalyticsRegistry
    {
        private readonly IDictionary<string, AnalyticsOperation> _analyticsRegistry;
        private readonly IDictionary<Guid, DateTime> _moduleLastHeardTime;
        public AnalyticsRegistry()
        {
            _analyticsRegistry = new Dictionary<string, AnalyticsOperation>();
            _moduleLastHeardTime = new Dictionary<Guid, DateTime>();
        }

        public void Register(AnalyticsModuleRegistrationModel module)
        {
            foreach (AnalyticsOperationModel operationModel in module.Operations) {
                if (_analyticsRegistry.ContainsKey(operationModel.Name))
                {
                    _analyticsRegistry[operationModel.Name].QueueName = module.QueueName;
                    if (!_analyticsRegistry[operationModel.Name].QueueNames.Contains(module.QueueName))
                    {
                        _analyticsRegistry[operationModel.Name].QueueNames.Add(module.QueueName);
                    }
                }
                else
                {
                    AnalyticsOperation operation = new AnalyticsOperation()
                    {
                        Name = operationModel.Name,
                        Description = operationModel.Description,
                        ModuleGuid = module.ModuleGuid,
                        QueueName = module.QueueName,
                        QueueNames = new List<string>()
                    };
                    operation.QueueNames.Add(module.QueueName);
                    _analyticsRegistry.Add(new KeyValuePair<string, AnalyticsOperation>(operation.Name, operation));
                }
            }
        }

        public string GetQueue(string name)
        {
            if (!_analyticsRegistry.ContainsKey(name)) return null;
            return _analyticsRegistry[name].QueueName;
        }

        public IList<string> GetQueues(string name)
        {
            if (!_analyticsRegistry.ContainsKey(name)) return null;
            return _analyticsRegistry[name].QueueNames;
        }


        
    }
}
