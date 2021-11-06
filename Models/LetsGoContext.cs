using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.Models
{
    public class LetsGoContext : IdentityDbContext<User>
    {
        public DbSet<User> ContextUsers { get; set; }
        public LetsGoContext(DbContextOptions options) : base(options) {}
    }
}
