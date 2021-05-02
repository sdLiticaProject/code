using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using sdLitica.Entities.Analytics;
using sdLitica.Entities.Management;
using sdLitica.Entities.TimeSeries;

namespace sdLitica.Relational.Context
{
    /// <summary>
    /// This class is used to configure MySql entities
    /// </summary>
    public class MySqlDbContext : DbContext
    {        
        /// <summary>
        /// Creates an MySqlDbContext class with predefined options
        /// </summary>
        /// <param name="contextOptions"></param>
        public MySqlDbContext(DbContextOptions<MySqlDbContext> contextOptions)
            : base(contextOptions)
        {

        }

        /// <summary>
        /// This method builds a mysql database model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAnalyticsOperation>()
                .Property(e => e.Arguments)
                .HasConversion(
                    p => JsonConvert.SerializeObject(p, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                    p => JObject.Parse(p)
                    );

            modelBuilder.Entity<AnalyticsModule>();
            modelBuilder.Entity<AnalyticsOperation>();

            modelBuilder.Entity<AnalyticsModulesOperations>().HasKey(mo => new { mo.AnalyticsModuleId, mo.AnalyticsOperationId });

            modelBuilder.Entity<AnalyticsModulesOperations>()
                .HasOne<AnalyticsModule>(sc => sc.AnalyticsModule)
                .WithMany(s => s.AnalyticsModulesOperations)
                .HasForeignKey(sc => sc.AnalyticsModuleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AnalyticsModulesOperations>()
                .HasOne<AnalyticsOperation>(sc => sc.AnalyticsOperation)
                .WithMany(s => s.AnalyticsModulesOperations)
                .HasForeignKey(sc => sc.AnalyticsOperationId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
                


            modelBuilder.Entity<User>();
            modelBuilder.Entity<UserToken>();
            modelBuilder.Entity<UserApiKey>();

            modelBuilder.Entity<TimeSeriesMetadata>();


            base.OnModelCreating(modelBuilder);
        }
    }
}
