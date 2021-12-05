using LetsGo.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.Subsidiary
{
    public class SuperAdminInitializer
    {
        public static async Task SeedAdminUser(
         RoleManager<IdentityRole> _roleManager,
         UserManager<User> _userManager)
        {
            string superAdminEmail = "superadmin@admin.com";
            string superAdminLogin = "superadmin";
            string superAdminPassword = "Password123!";
            var roles = new[] { "superadmin", "admin", "organiser"};
            foreach (var role in roles)
            {
                if (await _roleManager.FindByNameAsync(role) is null)
                    await _roleManager.CreateAsync(new IdentityRole(role));
            }

            if (await _userManager.FindByNameAsync(superAdminEmail) == null)
            {
                User superAdmin = new User { Email = superAdminEmail, UserName = superAdminLogin};
                superAdmin.EmailConfirmed = true;
                IdentityResult result = await _userManager.CreateAsync(superAdmin, superAdminPassword);
                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(superAdmin, "superadmin");
            }
        }
    }
}
