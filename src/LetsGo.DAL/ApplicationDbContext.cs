using System.Linq;
using LetsGo.Core.Entities;
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
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<LocationCategory> LocationCategories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventTicketType> EventTicketTypes { get; set; }
        public DbSet<PurchasedTicket> PurchasedTickets { get; set; }

        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Seed();

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

    //public static class ModelBuilderExtensions
    //{
    //    public static void Seed(this ModelBuilder modelBuilder)
    //    {
    //        modelBuilder.Entity<Author>().HasData(
    //            new Author
    //            {
    //                AuthorId = 1,
    //                FirstName = "William",
    //                LastName = "Shakespeare"
    //            }
    //        );
    //        modelBuilder.Entity<Book>().HasData(
    //            new Book { BookId = 1, AuthorId = 1, Title = "Hamlet" },
    //            new Book { BookId = 2, AuthorId = 1, Title = "King Lear" },
    //            new Book { BookId = 3, AuthorId = 1, Title = "Othello" }
    //        );
    //    }
    //}
}
