using LetsGo.DAL.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace LetsGo.DAL
{
    public class ApplicationDbContextFactory : IApplicationDbContextFactory
    {
        private readonly DbContextOptions _options;
        private readonly IWebHostEnvironment _appEnvironment;

        public ApplicationDbContextFactory(DbContextOptions options, IWebHostEnvironment appEnvironment)
        {
            _options = options;
            _appEnvironment = appEnvironment;
        }

        public ApplicationDbContext Create()
        {
            return new ApplicationDbContext(_options, _appEnvironment);
        }
    }
}