using System.IO;
using LetsGo.Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LetsGo.DAL.Extensions
{
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