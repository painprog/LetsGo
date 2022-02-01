using LetsGo.Core;
using LetsGo.Core.Entities;
using LetsGo.DAL.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace LetsGo.DAL.Extensions
{
    public static class AppDalConfiguringExtensions
    {
        public static void AddDataBase(this IServiceCollection services, IApplicationDbContextFactory applicationDbContextFactory)
        {
            services.AddScoped(sp => applicationDbContextFactory.Create());

            services
                .AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);

            services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddSingleton(sp => applicationDbContextFactory);
        }
    }
}