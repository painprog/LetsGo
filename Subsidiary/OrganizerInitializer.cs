using LetsGo.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace LetsGo.Subsidiary
{
    public class OrganizerInitializer
    {
        public static async Task SeedOrganizerRole(RoleManager<IdentityRole> _roleManager, UserManager<User> _userManager)
        {
            string organizerEmail = "organizer@admin.com";
            string organizerLogin = "organizer";
            string organizerPassword = "Password123!";
            var role = "organizer";

            if (await _roleManager.FindByNameAsync(role) is null)
                await _roleManager.CreateAsync(new IdentityRole(role));

            if (await _userManager.FindByNameAsync(organizerEmail) == null)
            {
                User organizer = new User { Email = organizerEmail, UserName = organizerLogin };
                IdentityResult result = await _userManager.CreateAsync(organizer, organizerPassword);
                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(organizer, "organizer");
            }
        }
    }
}