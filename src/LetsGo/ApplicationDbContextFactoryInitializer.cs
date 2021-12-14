using System;
using LetsGo.DAL;
using LetsGo.DAL.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace LetsGo
{
    public static class ApplicationDbContextFactoryInitializer
    {
        public static IApplicationDbContextFactory Create(string dbConnectionString, IWebHostEnvironment appEnvironment)
        {
            if (string.IsNullOrWhiteSpace(dbConnectionString))
                throw new ArgumentNullException(nameof(dbConnectionString));

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql(dbConnectionString, new MySqlServerVersion(new Version(8, 0)));
            return new ApplicationDbContextFactory(optionsBuilder.Options, appEnvironment);
        }
    }
}