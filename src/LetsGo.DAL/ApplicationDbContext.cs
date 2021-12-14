using System.IO;
using System.Linq;
using LetsGo.Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LetsGo.DAL
{
    public class ApplicationDbContext
        : IdentityDbContext<
            User,
            Role,
            int,
            IdentityUserClaim<int>,
            UserToRole,
            IdentityUserLogin<int>,
            IdentityRoleClaim<int>,
            IdentityUserToken<int>>
    {
        private IWebHostEnvironment _appEnvironment;

        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<LocationCategory> LocationCategories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventTicketType> EventTicketTypes { get; set; }
        public DbSet<PurchasedTicket> PurchasedTickets { get; set; }

        public ApplicationDbContext(DbContextOptions options, IWebHostEnvironment appEnvironment)
            : base(options)
        {
            _appEnvironment = appEnvironment;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Seed(_appEnvironment);

            DisableOneToManyCascadeDelete(builder);
        }

        private static void DisableOneToManyCascadeDelete(ModelBuilder builder)
        {
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }

    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder, IWebHostEnvironment appEnvironment)
        {
            modelBuilder.Entity<LocationCategory>().HasData(JsonConvert.DeserializeObject<LocationCategory[]>(File.ReadAllText(Path.Combine(appEnvironment.WebRootPath, "jsonsDataSeed/locationCategories.json"))));
            modelBuilder.Entity<EventCategory>().HasData(JsonConvert.DeserializeObject<EventCategory[]>(File.ReadAllText(Path.Combine(appEnvironment.WebRootPath, "jsonsDataSeed/eventCategories.json"))));
            modelBuilder.Entity<Location>().HasData(JsonConvert.DeserializeObject<Location[]>(File.ReadAllText(Path.Combine(appEnvironment.WebRootPath, "jsonsDataSeed/locations.json"))));
        }
    }
}
