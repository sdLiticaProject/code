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
        private readonly ModulesOperationsRepository _modulesOperationsRepository;
        public AnalyticsRegistry(AnalyticsOperationRepository analyticsOperationRepository, AnalyticsModuleRepository analyticsModuleRepository, ModulesOperationsRepository modulesOperationsRepository)
        {
            _analyticsOperationRepository = analyticsOperationRepository;
            _analyticsModuleRepository = analyticsModuleRepository;
            _modulesOperationsRepository = modulesOperationsRepository;
        }

        public void Register(AnalyticsModuleRegistrationModel module)
        {
            CheckAvailable();
            if (_analyticsModuleRepository.Contains(module.ModuleGuid))
            {
                AnalyticsModule analyticsModule_ = _analyticsModuleRepository.GetById(module.ModuleGuid);
                analyticsModule_.LastHeardTime = DateTime.Now;
                /*
                _analyticsModuleRepository.Update(new AnalyticsModule()
                {
                    Id = module.ModuleGuid,
                    LastHeardTime = DateTime.Now,
                    QueueName = module.QueueName
                });
                */
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
            _analyticsModuleRepository.SaveChanges();
            AnalyticsModule analyticsModule = _analyticsModuleRepository.GetById(module.ModuleGuid);
            foreach (AnalyticsOperationModel operationModel in module.Operations) {
                if (_analyticsOperationRepository.ContainsName(operationModel.Name))
                {
                    
                    AnalyticsOperation analyticsOperation = _analyticsOperationRepository.GetByName(operationModel.Name);
                    
                    if (!analyticsOperation.ModulesOperations.Select(mo => mo.AnalyticsModule).Contains(analyticsModule)) {
                        ModulesOperations moduleOperation = new ModulesOperations()
                        {
                            AnalyticsOperation = analyticsOperation,
                            AnalyticsModule = analyticsModule
                        };
                        _modulesOperationsRepository.Add(moduleOperation);
                        analyticsModule.ModulesOperations.Add(moduleOperation);
                        analyticsOperation.ModulesOperations.Add(moduleOperation);

                    }
                    /*
                    if (!analyticsOperation.AnalyticsModules.Contains(analyticsModule)) {
                        analyticsOperation.AnalyticsModules.Add(analyticsModule);
                    }*/
                }
                else
                {
                    AnalyticsOperation operation = new AnalyticsOperation()
                    {
                        Id = Guid.NewGuid(),
                        Name = operationModel.Name,
                        Description = operationModel.Description
                    };
                    //operation.AnalyticsModules.Add(analyticsModule);
                    ModulesOperations moduleOperation = new ModulesOperations()

                    {
                        AnalyticsOperation = operation,
                        AnalyticsModule = analyticsModule
                    };
                    _modulesOperationsRepository.Add(moduleOperation);
                    analyticsModule.ModulesOperations.Add(moduleOperation);
                    operation.ModulesOperations.Add(moduleOperation);

                    _analyticsOperationRepository.Add(operation);

                }
            }
            _analyticsOperationRepository.SaveChanges();
            _analyticsModuleRepository.SaveChanges();
            _modulesOperationsRepository.SaveChanges();
        }

        public string GetQueue(string name)
        {
            CheckAvailable();

            if (!_analyticsOperationRepository.ContainsName(name)) return null;
            return _analyticsOperationRepository.GetByName(name).ModulesOperations.Select(mo => mo.AnalyticsModule).First().QueueName; // maybe random
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
                    foreach (AnalyticsOperation operation in module.ModulesOperations.Select(mo => mo.AnalyticsOperation))
                    {
                        foreach (ModulesOperations mo in operation.ModulesOperations.Where(mo => mo.AnalyticsModule.Id.Equals(module.Id))) {
                            _modulesOperationsRepository.Delete(mo);
                        }
                        _analyticsOperationRepository.Delete(operation);
                    }
                    _analyticsModuleRepository.Delete(module);
                }
            }

            _analyticsModuleRepository.SaveChanges();
            _analyticsOperationRepository.SaveChanges();
            _modulesOperationsRepository.SaveChanges();

        }
        
    }
}
