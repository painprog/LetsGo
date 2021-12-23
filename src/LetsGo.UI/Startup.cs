using System;
using LetsGo.Core;
using LetsGo.Core.Entities;
using LetsGo.DAL;
using LetsGo.DAL.Contracts;
using LetsGo.UI.Extensions;
using LetsGo.UI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Globalization;

namespace LetsGo.UI
{
    public class Startup
    {
        private IApplicationDbContextFactory ApplicationDbContextFactory { get; }

        public IConfiguration Configuration { get; }


        public Startup(IConfiguration configuration, IWebHostEnvironment appEnvironment)
        {
            Configuration = configuration;

            ApplicationDbContextFactory = ApplicationDbContextFactoryInitializer.Create(
                Configuration.GetConnectionString("DefaultConnection"),
                appEnvironment
            );
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddControllersWithViews().AddViewLocalization().AddRazorRuntimeCompilation();
            services.AddTransient<EventsService>();
            services.AddTransient<LocationsService>();
            services.AddTransient<CabinetService>();
            services.AddTransient<EventsService>();

            services.AddScoped(sp => ApplicationDbContextFactory.Create());

            services
                .AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);

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
            var supportedCultures = new[]
            {
                new CultureInfo("ru"),
                new CultureInfo("en"),
                new CultureInfo("ky")
            }; 
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ru"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

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

            serviceProvider.InitializeUsersSeedData();
        }
    }
}
