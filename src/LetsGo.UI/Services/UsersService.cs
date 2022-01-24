using LetsGo.Core.Entities;
using LetsGo.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace LetsGo.UI.Services
{
    public class UsersService
    {
        private readonly ApplicationDbContext _context;
        private IMemoryCache _cache;
        private readonly UserManager<User> _userManager;
        public UsersService(IMemoryCache cache, ApplicationDbContext context, UserManager<User> userManager)
        {
            _cache = cache;
            _context = context;
            _userManager = userManager;
        }
        public async Task<User> Get(int id)
        {
            User user = null;
            if (!_cache.TryGetValue(id, out user))
            {
                user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user != null)
                {
                    _cache.Set(user.Id, user,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return user;
        }
    }
}
