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
            IConfiguration configuration = BootstrapSettings.AppSettings;
            string connectionString = configuration.GetConnectionString("MySql");
            ServerVersion serverVersion = ServerVersion.AutoDetect(connectionString);

            services.AddDbContextPool<MySqlDbContext>(options =>
            {
                options.UseMySql(connectionString, serverVersion, mysql =>
                {
                    //mysql.AnsiCharSet() etc
                });
            });
        }

        internal static void AddTimeSeries(this IServiceCollection services)
        {
            services.AddScoped<ITimeSeriesService, TimeSeriesService>();
        }
    }
}
