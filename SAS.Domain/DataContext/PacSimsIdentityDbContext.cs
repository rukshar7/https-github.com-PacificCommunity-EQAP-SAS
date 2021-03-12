using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SAS.Domain.Entities.Security;

namespace SAS.Domain.DataContext
{

    public class PacSimsIdentityDbContext : IdentityDbContext<ApplicationUser>
    {

        public PacSimsIdentityDbContext(DbContextOptions<PacSimsIdentityDbContext> options) : base(options)
        {
        }

        public DbSet<MenuMaster> MenuMaster { get; set; }
        public DbSet<MenuToRole> MenuToRole { get; set; }

        public class PacSimsIdentityDbContextDesignTimeFactory : IDesignTimeDbContextFactory<PacSimsIdentityDbContext>
        {
            public PacSimsIdentityDbContext CreateDbContext(string[] args)
            {
                var appConfig = new AppConfiguration();
                var optionsBuilder = new DbContextOptionsBuilder<PacSimsIdentityDbContext>();
                optionsBuilder.UseSqlServer(appConfig.GetPacSimsSecurityDbConnection);

                return new PacSimsIdentityDbContext(optionsBuilder.Options);
            }
        }
    }

}
