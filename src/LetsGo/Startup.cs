using System;
using System.Linq;
using System.Threading.Tasks;
using LetsGo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LetsGo.Core;
using LetsGo.Core.Entities;
using LetsGo.DAL;
using LetsGo.DAL.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LetsGo
{
    public class Startup
    {
        private IApplicationDbContextFactory ApplicationDbContextFactory { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment appEnvironment)
        {
            Configuration = configuration;

            ApplicationDbContextFactory = ApplicationDbContextFactoryInitializer.Create(
                Configuration.GetConnectionString("DefaultConnection"),
                appEnvironment
            );
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddTransient<EventsService>();
            services.AddTransient<LocationsService>();
            services.AddTransient<CabinetService>();
            services.AddTransient<EventsService>();

            services.AddScoped(sp => ApplicationDbContextFactory.Create());

            services
                .AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddSingleton(sp => ApplicationDbContextFactory);

            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            SampleData.Initialize(serviceProvider);
        }
    }

    public class SampleData
    {
        public static void Initialize(IServiceProvider services)
        {
            var context = services.GetService<ApplicationDbContext>();

            string[] roles = new string[] { "superadmin", "admin", "organizer" };

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
                SecurityStamp = Guid.NewGuid().ToString("D")
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

        public static async Task<IdentityResult> AssignRoles(IServiceProvider services, string email, string[] roles)
        {
            UserManager<User> _userManager = services.GetService<UserManager<User>>();
            User user = await _userManager.FindByEmailAsync(email);
            var result = await _userManager.AddToRolesAsync(user, roles);

            return result;
        }

    }
}
