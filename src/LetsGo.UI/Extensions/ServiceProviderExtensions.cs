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

            string[] roles = { "superadmin", "admin", "organizer", "usher", "apiclient" };

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
                Approved = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                AvatarLink = "/images/default_avatar.png",
                SelfInfo = string.Empty
            };

            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(user, superAdminPassword);
                user.PasswordHash = hashed;

                var userStore = new UserStore<User, Role, ApplicationDbContext, int>(context);

                userStore.CreateAsync(user).GetAwaiter().GetResult();
            }

            AssignRoles(services, user.Email, new[] { "superadmin", "apiclient" }).GetAwaiter().GetResult();

            context.SaveChangesAsync();
        }

        private static async Task AssignRoles(IServiceProvider services, string email, string[] roles)
        {
            UserManager<User> _userManager = services.GetService<UserManager<User>>();
            User user = await _userManager.FindByEmailAsync(email);

            foreach (string role in roles)
            {
                if (!await _userManager.IsInRoleAsync(user, role))
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }
        }

    }
}