using System;
using LetsGo.DAL;
using LetsGo.DAL.Contracts;
using LetsGo.UI.Extensions;
using LetsGo.UI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using LetsGo.DAL.Extensions;
using LetsGo.UI.HostedServices;
using LetsGo.UI.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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

            ApplyMigrations();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddControllersWithViews().AddViewLocalization().AddRazorRuntimeCompilation();
            services.AddTransient<EventsService>();
            services.AddTransient<LocationsService>();
            services.AddTransient<CabinetService>();
            services.AddTransient<EventsService>();
            services.AddTransient<UsersService>();

            services.AddDataBase(ApplicationDbContextFactory);

            services.AddRazorPages();

            services.AddHostedService<QueuedHostedService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddMemoryCache();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("ru"), new CultureInfo("ky") };

                options.DefaultRequestCulture = new RequestCulture(culture: "ru", uiCulture: "ru");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
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

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
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
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

            serviceProvider.InitializeUsersSeedData();
        }

        private void ApplyMigrations()
        {
            using (var context = ApplicationDbContextFactory.Create())
            {
                context.Database.SetCommandTimeout(TimeSpan.FromHours(4));
                context.Database.Migrate();
            }
        }
    }
}
