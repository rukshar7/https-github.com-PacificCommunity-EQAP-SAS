using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
//using SAS.Web.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SAS.Application.Interface;
using SAS.Domain.DataContext;
using SAS.Domain.Entities.Security;
using SAS.Application.Concrete;

namespace SAS.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            //services.AddRazorPages();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddHttpContextAccessor();


            services.AddDbContext<SASDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("SASDbConnection")));

            services.AddDbContext<PacSimsIdentityDbContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("PacSimsSecurityDbConnection")));


            services.AddScoped<DbContext, SASDbContext>();

            // Configure Identity db
            services.AddIdentity<ApplicationUser, IdentityRole>(
                 options => options.Stores.MaxLengthForKeys = 128)
                .AddEntityFrameworkStores<PacSimsIdentityDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.AddScoped<ISecurityLogic, SecurityLogic>();

            services.AddControllersWithViews();

            services.AddAuthorization();


            services.AddControllers();

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            PacSimsIdentityInitializer.SeedData(userManager, roleManager);
            app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapRazorPages();
            //});


            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                 name: "default",
                 pattern: "{controller=Home}/{action=Index}/{id?}");

                //endpoints.MapAreaControllerRoute(
                // name: "areas", "areas",
                // pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();


                //app.UseEndpoints(endpoints =>
                //{
                //    endpoints.MapControllerRoute(
                //        name: "default",
                //        pattern: "{controller=Home}/{action=Index}/{id?}");
                //});

            });
        }
    }
}
