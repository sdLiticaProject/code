using sdLitica.Entities.Analytics;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Bus;
using sdLitica.Events.Integration;
using sdLitica.Relational.Repositories;
using sdLitica.Utils.Abstractions;
using sdLitica.Utils.Models;
using sdLitica.Utils.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sdLitica.AnalyticsManagementCore
{
    /// <summary>
    /// This service manages available analytical modules and operations. 
    /// </summary>
    public class AnalyticsRegistry
    {
        private readonly AnalyticsSettings _analyticsSettings;
        private readonly AnalyticsOperationRepository _analyticsOperationRepository;
        private readonly AnalyticsModuleRepository _analyticsModuleRepository;
        private readonly ModuleOperationRepository _modulesOperationsRepository;
        public AnalyticsRegistry(IAppSettings _AppSettings, AnalyticsOperationRepository analyticsOperationRepository, AnalyticsModuleRepository analyticsModuleRepository, ModuleOperationRepository modulesOperationsRepository)
        {
            _analyticsSettings = _AppSettings.AnalyticsSettings;
            _analyticsOperationRepository = analyticsOperationRepository;
            _analyticsModuleRepository = analyticsModuleRepository;
            _modulesOperationsRepository = modulesOperationsRepository;
        }

        /// <summary>
        /// Register analytical module
        /// </summary>
        /// <param name="module"></param>
        public void Register(AnalyticsModuleRegistrationModel module)
        {
            CheckAvailable();
            if (_analyticsModuleRepository.Contains(module.ModuleGuid))
            {
                AnalyticsModule analyticsModule_ = _analyticsModuleRepository.GetById(module.ModuleGuid);
                analyticsModule_.LastHeardTime = DateTime.Now;
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
                }
                else
                {
                    AnalyticsOperation operation = new AnalyticsOperation()
                    {
                        Id = Guid.NewGuid(),
                        Name = operationModel.Name,
                        Description = operationModel.Description
                    };
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
            //_analyticsModuleRepository.SaveChanges();
            //_modulesOperationsRepository.SaveChanges();
        }

        /// <summary>
        /// Get rabbitmq-queue for module that can execute operation given by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetQueue(string name)
        {
            CheckAvailable();

            if (!_analyticsOperationRepository.ContainsName(name)) return null;
            IList<ModulesOperations> modulesOperations = _analyticsOperationRepository.GetByName(name).ModulesOperations;
            if (modulesOperations == null || modulesOperations.Count < 1) return null;
            return modulesOperations.Select(mo => mo.AnalyticsModule).First().QueueName; 
        }

        /// <summary>
        /// Get list of available analytical operations. 
        /// </summary>
        /// <returns></returns>
        public IList<AnalyticsOperationModel> GetAvailableOperations()
        {
            CheckAvailable();

            IList<AnalyticsOperationModel> operations = new List<AnalyticsOperationModel>();
            foreach (AnalyticsOperation operation in _analyticsOperationRepository.GetAll())
            {
                IList<ModulesOperations> modulesOperations = operation.ModulesOperations;
                if (modulesOperations != null && modulesOperations.Count > 0)
                {
                    operations.Add(new AnalyticsOperationModel()
                    {
                        Name = operation.Name,
                        Description = operation.Description
                    });
                }
            }
            return operations;
        }

        /// <summary>
        /// Delete disabled analytical modules and its operations. 
        /// </summary>
        public void CheckAvailable()
        {
            List<Guid> modulesToRemove = new List<Guid>();
            foreach (AnalyticsModule module in _analyticsModuleRepository.GetAll())
            {
                if (DateTime.Now - module.LastHeardTime > new TimeSpan(0, 0, 0, 0, milliseconds: _analyticsSettings.ModuleDeadTimeout))
                {
                    _analyticsModuleRepository.Delete(module);
                }
            }

            _analyticsModuleRepository.SaveChanges();
            //_analyticsOperationRepository.SaveChanges();
            //_modulesOperationsRepository.SaveChanges();

        }
        
    }
}
