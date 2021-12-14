using LetsGo.Core.Entities;
using LetsGo.Core.Repositories;

namespace LetsGo.DAL.Repositories
{
    public class LocationCategoryRepository : Repository<LocationCategory>, ILocationCategoryRepository
    {
        public LocationCategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}