using Microsoft.EntityFrameworkCore;
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
            modelBuilder.Entity<UserAnalyticsOperation>();

            modelBuilder.Entity<AnalyticsModule>();
            modelBuilder.Entity<AnalyticsOperation>();

            modelBuilder.Entity<ModulesOperations>().HasKey(mo => new { mo.AnalyticsModuleId, mo.AnalyticsOperationId });

            modelBuilder.Entity<ModulesOperations>()
                .HasOne<AnalyticsModule>(sc => sc.AnalyticsModule)
                .WithMany(s => s.ModulesOperations)
                .HasForeignKey(sc => sc.AnalyticsModuleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ModulesOperations>()
                .HasOne<AnalyticsOperation>(sc => sc.AnalyticsOperation)
                .WithMany(s => s.ModulesOperations)
                .HasForeignKey(sc => sc.AnalyticsOperationId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
                


            modelBuilder.Entity<User>();
            modelBuilder.Entity<UserToken>();

            modelBuilder.Entity<TimeSeriesMetadata>();


            base.OnModelCreating(modelBuilder);
        }
    }
}
