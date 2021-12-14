using System.Threading.Tasks;
using LetsGo.Core.Entities;
using LetsGo.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LetsGo.DAL.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<Role> GetByNameAsync(string name)
        {
            return GetAll()
                .SingleOrDefaultAsync(r => r.NormalizedName == name.ToUpper());
        }
    }
}