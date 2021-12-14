using System.Threading.Tasks;
using LetsGo.Core.Entities;

namespace LetsGo.Core.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role> GetByNameAsync(string name);
    }
}