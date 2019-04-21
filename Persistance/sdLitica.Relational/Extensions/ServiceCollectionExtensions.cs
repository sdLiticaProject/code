using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using sdLitica.Relational.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Relational.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRelationalDatabase(this IServiceCollection serviceDescriptors, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MySql");
            serviceDescriptors.AddDbContextPool<MySqlDbContext>(options =>
            {
                options.UseMySql(connectionString, mysql => 
                {
                    
                });                
            });
        }
    }
}
