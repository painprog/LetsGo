using System.Collections.Generic;
using System.Threading.Tasks;
using LetsGo.Core.Entities;

namespace LetsGo.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetByNameAsync(string userName);
    }
}