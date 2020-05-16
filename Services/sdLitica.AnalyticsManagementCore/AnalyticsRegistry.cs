using sdLitica.Events.Abstractions;
using sdLitica.Events.Bus;
using sdLitica.Events.Integration;
using sdLitica.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (_moduleLastHeardTime.ContainsKey(module.ModuleGuid))
            {
                _moduleLastHeardTime[module.ModuleGuid] = DateTime.Now;
            }
            else
            {
                _moduleLastHeardTime.Add(module.ModuleGuid, DateTime.Now);
            }
            

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
            List<Guid> modulesToRemove = new List<Guid>();
            foreach (var moduleGuid in _moduleLastHeardTime.Keys)
            {
                if (DateTime.Now - _moduleLastHeardTime[moduleGuid] > new TimeSpan(0, 0, 15))
                {
                    System.Console.WriteLine(DateTime.Now);
                    System.Console.WriteLine(_moduleLastHeardTime[moduleGuid]);
                    List<string> registryToRemove = _analyticsRegistry.Where(pair => pair.Value.ModuleGuid.Equals(moduleGuid)).Select(pair => pair.Key).ToList();
                    foreach (var key in registryToRemove) _analyticsRegistry.Remove(key);

                    modulesToRemove.Add(moduleGuid);
                }
            }
            foreach (var key in modulesToRemove) _moduleLastHeardTime.Remove(key);

            if (!_analyticsRegistry.ContainsKey(name)) return null;
            return _analyticsRegistry[name].QueueName;
        }

        public IList<string> GetQueues(string name)
        {
            List<Guid> modulesToRemove = new List<Guid>();
            foreach (var moduleGuid in _moduleLastHeardTime.Keys)
            {
                if (DateTime.Now - _moduleLastHeardTime[moduleGuid] > new TimeSpan(0, 0, 15))
                {
                    List<string> registryToRemove = _analyticsRegistry.Where(pair => pair.Value.ModuleGuid.Equals(moduleGuid)).Select(pair => pair.Key).ToList();
                    foreach (var key in registryToRemove) _analyticsRegistry.Remove(key);

                    modulesToRemove.Add(moduleGuid);
                }
            }
            foreach (var key in modulesToRemove) _moduleLastHeardTime.Remove(key);


            if (!_analyticsRegistry.ContainsKey(name)) return null;
            return _analyticsRegistry[name].QueueNames;
        }


        
    }
}
