using Microsoft.EntityFrameworkCore;
using sdLitica.Analytics;
using sdLitica.Entities.Management;

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
            modelBuilder.Entity<AnalyticsOperationRequest>();

            modelBuilder.Entity<User>();
            modelBuilder.Entity<UserToken>(b =>
            {
                b.Property(p => p.TokenExpirationDate).HasColumnName("expiration");
                b.ToTable("user_token");
            });


            base.OnModelCreating(modelBuilder);
        }
    }
}
