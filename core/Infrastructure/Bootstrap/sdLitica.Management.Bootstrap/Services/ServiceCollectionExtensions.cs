using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using sdLitica.AnalyticsManagementCore;
using sdLitica.Entities.Management.Repositories;
using sdLitica.PlatformCore;
using sdLitica.Relational.Repositories;
using sdLitica.TimeSeries.Services;

namespace sdLitica.Bootstrap.Services
{
    internal static class ServiceCollectionExtensions
    {
        internal static void AddManagementServices(this IServiceCollection services)
        {
            services.AddScoped<UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserTokenRepository, UserTokenRepository>();
            services.AddScoped<IUserApiKeyRepository, UserApiKeyRepository>();

            services.AddScoped<TimeSeriesMetadataRepository>();
            services.AddScoped<ITimeSeriesMetadataService, TimeseriesMetadataService>();

            services.AddScoped<AnalyticsOperationRequestRepository>();
            services.AddScoped<AnalyticsModuleRepository>();
            services.AddScoped<AnalyticsOperationRepository>();
            services.AddScoped<ModuleOperationRepository>();

            services.AddScoped<AnalyticsRegistry>();
            services.AddScoped<AnalyticsService>();
        }
    }
}
