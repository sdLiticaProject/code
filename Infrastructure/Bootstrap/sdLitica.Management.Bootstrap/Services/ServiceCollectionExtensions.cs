using Microsoft.Extensions.DependencyInjection;
using sdLitica.AnalyticsManagementCore;
using sdLitica.Entities.Management.Repositories;
using sdLitica.PlatformCore;
using sdLitica.Relational.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Bootstrap.Services
{
    internal static class ServiceCollectionExtensions
    {
        internal static void AddManagementServices(this IServiceCollection services)
        {
            services.AddScoped<UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserTokenRepository, UserTokenRepository>();

            services.AddScoped<AnalyticsService>();
        }
    }
}
