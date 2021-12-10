using LetsGo.Subsidiary;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.Models
{
    public class LetsGoContext : IdentityDbContext<User>
    {
        private IWebHostEnvironment _appEnvironment;

        public DbSet<User> ContextUsers { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<LocationCategory> LocationCategories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventTicketType> EventTicketTypes { get; set; }
        public DbSet<PurchasedTicket> PurchasedTickets { get; set; }
        public LetsGoContext(DbContextOptions options, IWebHostEnvironment appEnvironment) : base(options)
        {
            _appEnvironment = appEnvironment;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            string filepath = Path.Combine(_appEnvironment.WebRootPath, "jsonsDataSeed/locationCategories.json");
            builder.Entity<LocationCategory>().HasData(
                JsonReader.ReadJson<LocationCategory>(filepath)
                );

            filepath = Path.Combine(_appEnvironment.WebRootPath, "jsonsDataSeed/eventCategories.json");
            builder.Entity<EventCategory>().HasData(
                JsonReader.ReadJson<EventCategory>(filepath)
                );

            filepath = Path.Combine(_appEnvironment.WebRootPath, "jsonsDataSeed/locations.json");
            builder.Entity<Location>().HasData(
                JsonReader.ReadJson<Location>(filepath)
                );
        }
    }
}
