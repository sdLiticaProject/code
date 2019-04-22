using Microsoft.Extensions.DependencyInjection;
using sdLitica.Bootstrap.Persistance;
using sdLitica.Bootstrap.Services;

namespace sdLitica.Bootstrap.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddManagementServices();                       
        }

        public static void AddRelationalDatabase(this IServiceCollection services)
        {
            services.AddMySql();
        }
    }
}
