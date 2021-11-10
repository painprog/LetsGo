using LetsGo.Models;
using LetsGo.Services;
using LetsGo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LetsGo.Controllers
{
    public class EventController : Controller
    {
        private readonly EventsService _Service;
        private readonly LetsGoContext _goContext;

        public EventController(EventsService service, LetsGoContext goContext)
        {
            _Service = service;
            _goContext = goContext;
        }

        public async Task<IActionResult> Details()
        {
            //Event @event = _Service.GetEvent(id).Result;
            //return View(@event);

            Event @event = new Event {
                Id = Guid.NewGuid().ToString(),
                Name = "Концерт «Интерстеллар. Simple Music Ensemble»",
                Description = "Музыка из культового фильма «Интерстеллар» среди растений – 26 сентября! Автор завораживающих саундтреков к фантастическому эпосу Кристофера Нолана про задыхающуюся Землю, космические полеты и парадоксы времени – один из величайших кинокомпозиторов современности Ханс Циммер, лауреат премии «Оскар», двукратный лауреат «Золотого глобуса», трехкратный лауреат «Грэмми».",
                CreatedAt = DateTime.Now,
                EventStart = new DateTime(2021, 11, 19),
                EventEnd = new DateTime(2021, 11, 19),
                PosterImage = "10101010011001.jpeg",
                Categories = JsonSerializer.Serialize(new List<EventCategory> {
                    new EventCategory { Name = "классика" },
                    new EventCategory { Name = "инструментальная музыка" }
                }),
                //Categories = "{ Name: \"классика\", Name: \"инструментальная музыка\" }",
                AgeLimit = 0,
                TicketLimit = 450,
                LocationId = "local",
                Location = new Location {
                    Id = Guid.NewGuid().ToString(),
                    Name = "ВДНХ (Выставка достижений народного хозяйства)",
                    Address = "пр-т Мира, 119, Москва, Россия, 129223",
                    Phones = "",
                    Description = "Вы́ставка достиже́ний наро́дного хозя́йства — выставочный комплекс в Останкинском районе Северо-Восточного административного округа города Москвы, второй по величине выставочный комплекс в городе. Входит в 50 крупнейших выставочных центров мира.",
                    Categories = JsonSerializer.Serialize(new List<LocationCategory> { 
                        new LocationCategory{ Name = "выставочный центр" }
                    })
                }
            };


            DetailsViewModel viewModel = new DetailsViewModel {
                Event = @event,
                LocationCategories = JsonSerializer.Deserialize<List<LocationCategory>>(@event.Location.Categories),
                EventCategories = JsonSerializer.Deserialize<List<EventCategory>>(@event.Categories)
            };

            return View(viewModel);
        }
    }
}
