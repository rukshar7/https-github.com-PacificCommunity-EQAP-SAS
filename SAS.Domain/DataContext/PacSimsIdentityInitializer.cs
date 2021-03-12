using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using SAS.Domain.Entities.Security;


namespace SAS.Domain.DataContext
{

    public class PacSimsIdentityInitializer
    {
        public static void SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);

        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByNameAsync("eqaphelpdesk@spc.int").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "eqaphelpdesk@spc.int";
                user.Email = "eqaphelpdesk@spc.int";

                IdentityResult result = userManager.CreateAsync(user, "H3lpd3$k202@").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Administrator";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }

        }
    }

}