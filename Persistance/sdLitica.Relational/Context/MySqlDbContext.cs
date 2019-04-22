using Microsoft.EntityFrameworkCore;
using sdLitica.Entities.Management;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Relational.Context
{
    public class MySqlDbContext : DbContext
    {
        public MySqlDbContext(DbContextOptions<MySqlDbContext> contextOptions)
            : base(contextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
