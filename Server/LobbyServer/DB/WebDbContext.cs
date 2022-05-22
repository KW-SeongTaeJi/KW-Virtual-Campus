using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LobbyServer.DB
{
    public class WebDbContext : DbContext
    {
        public DbSet<UserAccountDb> UserAccounts { get; set; }

        string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AccountDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //string _connectionString = @"Data Source=kwcampus-db.cixwp6zptiuf.ap-northeast-2.rds.amazonaws.com,1433;Initial Catalog=AccountDB;User id=admin;Password=rlaalsxo1223;";


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options
                .UseSqlServer(_connectionString);
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
