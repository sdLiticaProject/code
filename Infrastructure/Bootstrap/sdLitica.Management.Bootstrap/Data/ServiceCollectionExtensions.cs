using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using sdLitica.Bootstrap.Settings;
using sdLitica.Relational.Context;
using sdLitica.Relational.Repositories;
using sdLitica.TimeSeries.Services;

namespace sdLitica.Bootstrap.Data
{
    internal static class ServiceCollectionExtensions
    {
        internal static void AddMySql(this IServiceCollection services)
        {
            IConfiguration configuration = BootstrapSettings.AppSettings;
            string connectionString = configuration.GetConnectionString("MySql");
            
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
            services.AddScoped<ITimeSeriesService, TimeSeriesService>();
            services.AddScoped<TimeSeriesMetadataRepository>();
            services.AddScoped<ITimeSeriesMetadataService, TimeseriesMetadataService>();
        }
    }
}
