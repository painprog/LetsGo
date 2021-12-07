using LetsGo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.Subsidiary
{
    public class LocationsInitializer
    {
        public static async Task LocationsSeed(LetsGoContext db)
        {
            if (!db.Locations.Any())
            {
                await db.Locations.AddAsync(new Location { Name = "Асанбай Центр", Address = "21, 11 Аалы Токомбаева көчөсү, Бишкек", Phones = "[\"+996775979500\"]", Description= "Художественный центр в Бишкеке", Categories= "[{\"Id\":\"-\",\"Name\":\"-\"}]" });
                await db.Locations.AddAsync(new Location { Name = "Ретро-Метро", Address = "24 просп. Мира, Бишкек", Phones = "[\"+996705 000 888\"]", Description = "Концертный зал", Categories = "[{\"Id\":\"-\",\"Name\":\"-\"}]" });
                await db.Locations.AddAsync(new Location { Name = "Дворец спорта имени Кожомкула", Address = "501 улица Фрунзе, Бишкек", Phones = "[\"+996775979500\"]", Description = "Спорт комплекс", Categories = "[{\"Id\":\"-\",\"Name\":\"-\"}]" });
                await db.Locations.AddAsync(new Location { Name = "Площадь Ала-Тоо", Address = "просп. Чуй, Бишкек", Phones = "[\"-\"]", Description = "Центральная площадь", Categories = "[{\"Id\":\"-\",\"Name\":\"-\"}]" });
                await db.Locations.AddAsync(new Location { Name = "Стадион Спартак", Address = "17 ул. Тоголок Молдо, Бишкек", Phones = "[\"-\"]", Description = "Стадион", Categories = "[{\"Id\":\"-\",\"Name\":\"-\"}]" });
                await db.Locations.AddAsync(new Location { Name = "Опера Жана Балет Театры", Address = "167 Советская, Бишкек", Phones = "[\"0312 621 619\"]", Description = "Театр блин оперы и балета", Categories = "[{\"Id\":\"-\",\"Name\":\"-\"}]" });
                await db.SaveChangesAsync();
            }
        }
    }
}
