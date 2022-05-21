using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountServer.DB
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserAccountDb> Accounts { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserAccountDb>()
                .HasIndex(a => a.AccountId)
                .IsUnique();

            builder.Entity<UserAccountDb>()
                .HasIndex(a => a.Name)
                .IsUnique();
        }
    }
}
