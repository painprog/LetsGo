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
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<LocationCategory> LocationCategories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventTicketType> EventTicketTypes { get; set; }
        public LetsGoContext(DbContextOptions options) : base(options) {}
    }
}
