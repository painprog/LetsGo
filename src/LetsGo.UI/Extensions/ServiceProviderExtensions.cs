using System;
using System.Linq;
using System.Threading.Tasks;
using LetsGo.Core.Entities;
using LetsGo.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LetsGo.UI.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void InitializeUsersSeedData(this IServiceProvider services)
        {
            var context = services.GetService<ApplicationDbContext>();

            string[] roles = { "superadmin", "admin", "organizer", "usher" };

            foreach (string role in roles)
            {
                var roleStore = new RoleStore<Role, ApplicationDbContext, int>(context);

                if (!context.Roles.Any(r => r.Name == role))
                {
                    roleStore.CreateAsync(new Role(role)).GetAwaiter().GetResult();
                }
            }

            string superAdminEmail = "superadmin@admin.com";
            string superAdminLogin = "superadmin";
            string superAdminPassword = "Password123!";

            var user = new User
            {
                Email = superAdminEmail,
                NormalizedEmail = superAdminEmail.ToUpper(),
                UserName = superAdminLogin,
                NormalizedUserName = superAdminLogin.ToUpper(),
                PhoneNumber = string.Empty,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                AvatarLink = "/images/default_avatar.png"
            };

            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(user, superAdminPassword);
                user.PasswordHash = hashed;

                var userStore = new UserStore<User, Role, ApplicationDbContext, int>(context);

                userStore.CreateAsync(user).GetAwaiter().GetResult();
            }

            AssignRoles(services, user.Email, new[] { "superadmin" }).GetAwaiter().GetResult();

            context.SaveChangesAsync();
        }

        private static async Task<IdentityResult> AssignRoles(IServiceProvider services, string email, string[] roles)
        {
            UserManager<User> _userManager = services.GetService<UserManager<User>>();
            User user = await _userManager.FindByEmailAsync(email);
            var result = await _userManager.AddToRolesAsync(user, roles);

            return result;
        }

    }
}