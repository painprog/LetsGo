using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetsGo.Core.Entities;
using LetsGo.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LetsGo.DAL.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<List<User>> GetAllAsync()
        {
            return GetAll().ToListAsync();
        }

        public Task<User> GetByNameAsync(string userName)
        {
            return GetAll().FirstOrDefaultAsync(u => u.UserName == userName);
        }
    }
}