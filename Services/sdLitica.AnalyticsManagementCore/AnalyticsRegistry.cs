using sdLitica.Analytics;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Bus;
using sdLitica.Events.Integration;
using sdLitica.Relational.Repositories;
using sdLitica.Utils.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sdLitica.AnalyticsManagementCore
{
    public class AnalyticsRegistry
    {
        private readonly AnalyticsOperationRepository _analyticsOperationRepository;
        private readonly AnalyticsModuleRepository _analyticsModuleRepository;
        public AnalyticsRegistry(AnalyticsOperationRepository analyticsOperationRepository, AnalyticsModuleRepository analyticsModuleRepository)
        {
            _analyticsOperationRepository = analyticsOperationRepository;
            _analyticsModuleRepository = analyticsModuleRepository;
        }

        public void Register(AnalyticsModuleRegistrationModel module)
        {
            if (_analyticsModuleRepository.Contains(module.ModuleGuid))
            {
                _analyticsModuleRepository.Update(new AnalyticsModule()
                {
                    Id = module.ModuleGuid,
                    LastHeardTime = DateTime.Now,
                    QueueName = module.QueueName
                });
            }
            else
            {
                _analyticsModuleRepository.Add(new AnalyticsModule()
                {
                    Id = module.ModuleGuid,
                    LastHeardTime = DateTime.Now,
                    QueueName = module.QueueName
                });
            }
            AnalyticsModule analyticsModule = _analyticsModuleRepository.GetById(module.ModuleGuid);
            foreach (AnalyticsOperationModel operationModel in module.Operations) {
                if (_analyticsOperationRepository.ContainsName(operationModel.Name))
                {
                    AnalyticsOperation analyticsOperation = _analyticsOperationRepository.GetByName(operationModel.Name);
                    analyticsOperation.Module = analyticsModule;
                }
                else
                {
                    AnalyticsOperation operation = new AnalyticsOperation()
                    {
                        Id = Guid.NewGuid(),
                        Name = operationModel.Name,
                        Description = operationModel.Description,
                        Module = analyticsModule
                    };
                    _analyticsOperationRepository.Add(operation);

                }
            }
            _analyticsOperationRepository.SaveChanges();
            _analyticsModuleRepository.SaveChanges();
        }

        public string GetQueue(string name)
        {
            CheckAvailable();

            if (!_analyticsOperationRepository.ContainsName(name)) return null;
            return _analyticsOperationRepository.GetByName(name).Module.QueueName;
        }


        public IList<AnalyticsOperationModel> GetAvailableOperations()
        {
            CheckAvailable();

            IList<AnalyticsOperationModel> operations = new List<AnalyticsOperationModel>();
            foreach (AnalyticsOperation operation in _analyticsOperationRepository.GetAll())
            {
                operations.Add(new AnalyticsOperationModel()
                {
                    Name = operation.Name,
                    Description = operation.Description
                });
            }
            return operations;
        }

        public void CheckAvailable()
        {
            List<Guid> modulesToRemove = new List<Guid>();
            foreach (AnalyticsModule module in _analyticsModuleRepository.GetAll())
            {
                if (DateTime.Now - module.LastHeardTime > new TimeSpan(0,0,15))
                {
                    foreach (AnalyticsOperation operation in _analyticsOperationRepository.GetAll())
                    {
                        _analyticsOperationRepository.Delete(operation);
                    }
                    _analyticsModuleRepository.Delete(module);
                }
            }

            _analyticsModuleRepository.SaveChanges();
            _analyticsOperationRepository.SaveChanges();

        }
        
    }
}
