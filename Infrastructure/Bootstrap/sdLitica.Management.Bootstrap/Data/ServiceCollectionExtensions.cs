using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using sdLitica.Bootstrap.Settings;
using sdLitica.Relational.Context;
using sdLitica.TimeSeries.Services;

namespace sdLitica.Bootstrap.Data
{
    internal static class ServiceCollectionExtensions
    {
        internal static void AddMySql(this IServiceCollection services)
        {
            var configuration = BootstrapSettings.AppSettings;
            var connectionString = configuration.GetConnectionString("MySql");

            services.AddDbContextPool<MySqlDbContext>(options =>
            {
                options.UseMySql(connectionString, mysql =>
                {
                    //mysql.AnsiCharSet() etc
                });
            });
        }

        internal static void AddTimeSeries(this IServiceCollection services)
        {
            services.AddSingleton<ITimeSeriesService, TimeSeriesService>();//adscoped
        }
    }
}
