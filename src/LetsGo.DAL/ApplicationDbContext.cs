using System.Linq;
using LetsGo.Core.Entities;
using LetsGo.DAL.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
        private readonly IWebHostEnvironment _appEnvironment;

        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<LocationCategory> LocationCategories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventTicketType> EventTicketTypes { get; set; }
        public DbSet<PurchasedTicket> PurchasedTickets { get; set; }

        public ApplicationDbContext(DbContextOptions options, IWebHostEnvironment appEnvironment) : base(options)
        {
            _appEnvironment = appEnvironment;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            if (!string.IsNullOrWhiteSpace(_appEnvironment.WebRootPath))
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
}
