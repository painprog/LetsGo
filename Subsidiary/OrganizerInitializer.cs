using LetsGo.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace LetsGo.Subsidiary
{
    public class OrganizerInitializer
    {
        public static async Task SeedOrganizerRole(RoleManager<IdentityRole> _roleManager)
        {
            var role = "organizer";

            if (await _roleManager.FindByNameAsync(role) is null)
                await _roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}