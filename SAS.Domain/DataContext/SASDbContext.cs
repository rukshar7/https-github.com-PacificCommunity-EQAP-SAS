
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAS.Domain.DataContext
{
    public class SASDbContext : DbContext
    {

        public SASDbContext(DbContextOptions<SASDbContext> options)
          : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public class SASDbContextDesignTimeFactory : IDesignTimeDbContextFactory<SASDbContext>
        {
            public SASDbContext CreateDbContext(string[] args)
            {
                var appConfig = new AppConfiguration();
                var optionsBuilder = new DbContextOptionsBuilder<SASDbContext>();
                optionsBuilder.UseSqlServer(appConfig.GetSASDbConnection);

                return new SASDbContext(optionsBuilder.Options);
            }
        }

    }
}
