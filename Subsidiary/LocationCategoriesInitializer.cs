using LetsGo.Models;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.Subsidiary
{
    public class LocationCategoriesInitializer
    {
        public static async Task LocationCategoriesSeed(LetsGoContext db)
        {
            if (!db.LocationCategories.Any())
            {
                await db.LocationCategories.AddAsync(new LocationCategory { Name = "Театры" });
                await db.LocationCategories.AddAsync(new LocationCategory { Name = "Музеи" });
                await db.LocationCategories.AddAsync(new LocationCategory { Name = "Выставочные центры" });
                await db.LocationCategories.AddAsync(new LocationCategory { Name = "Кинотеатры" });
                await db.LocationCategories.AddAsync(new LocationCategory { Name = "Спортивные комплексы" });
                await db.LocationCategories.AddAsync(new LocationCategory { Name = "Галереи" });
                await db.LocationCategories.AddAsync(new LocationCategory { Name = "Клубы" });
                await db.LocationCategories.AddAsync(new LocationCategory { Name = "Бары" });
                await db.LocationCategories.AddAsync(new LocationCategory { Name = "Кофейни" });
                await db.LocationCategories.AddAsync(new LocationCategory { Name = "Другое" });
                await db.SaveChangesAsync();
            }
        }
    }
}
