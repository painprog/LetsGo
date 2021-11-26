using LetsGo.Models;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.Subsidiary
{
    public class CategoriesInitializer
    {
        public static async Task CategoriesSeed(LetsGoContext db)
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

            if(!db.EventCategories.Any())
            {
                await db.EventCategories.AddAsync(new EventCategory { Name = "Концерты" });
                await db.EventCategories.AddAsync(new EventCategory { Name = "Фестивали" });
                await db.EventCategories.AddAsync(new EventCategory { Name = "Спектакли" });
                await db.EventCategories.AddAsync(new EventCategory { Name = "Детям" });
                await db.EventCategories.AddAsync(new EventCategory { Name = "Классика" });
                await db.EventCategories.AddAsync(new EventCategory { Name = "Экскурсии" });
                await db.EventCategories.AddAsync(new EventCategory { Name = "Другое" });
                await db.SaveChangesAsync();
            }
        }
    }
}
