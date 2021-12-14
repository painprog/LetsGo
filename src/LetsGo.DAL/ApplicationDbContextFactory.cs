using LetsGo.DAL.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LetsGo.DAL
{
    public class ApplicationDbContextFactory : IApplicationDbContextFactory
    {
        private readonly DbContextOptions _options;

        public ApplicationDbContextFactory(DbContextOptions options)
        {
            _options = options;
        }

        public ApplicationDbContext Create()
        {
            return new ApplicationDbContext(_options);
        }
    }
}