using Microsoft.Extensions.DependencyInjection;
using sdLitica.Entities.Management.Repositories;
using sdLitica.Relational.Repositories;
using sdLitica.Services.Management;
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
        }
    }
}
