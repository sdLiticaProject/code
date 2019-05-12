using Microsoft.Extensions.DependencyInjection;
using sdLitica.Bootstrap.Data;
using sdLitica.Bootstrap.Services;
using sdLitica.Utils.Abstractions;
using sdLitica.Utils.Settings;

namespace sdLitica.Bootstrap.Extensions
{
    /// <summary>
    /// Extensions class for the IServiceCollection class
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// This method add application services in the dependency injection container
        /// </summary>
        /// <param name="services"></param>
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAppSettings, AppSettings>();
            services.AddManagementServices();                       
        }

        /// <summary>
        /// This method adds relational database services in the dependency injection container
        /// </summary>
        /// <param name="services"></param>
        public static void AddRelationalDatabase(this IServiceCollection services)
        {
            services.AddMySql();
        }
    }
}
