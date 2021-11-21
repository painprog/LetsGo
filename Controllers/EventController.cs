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

        public async Task<IActionResult> Details(string id)
        {
            Event @event = _Service.GetEvent(id).Result;
            var tickets = new List<EventTicketType> {
                    new EventTicketType { Name = "vip", EventId = @event.Id, Count = 50, Sold = 0, Price = 800, Id = Guid.NewGuid().ToString() },
                    new EventTicketType { Name = "classic", EventId = @event.Id, Count = 80, Sold = 0, Price = 400, Id = Guid.NewGuid().ToString() }
                };
            DetailsViewModel viewModel = new DetailsViewModel {
                Event = @event,
                LocationCategories = JsonSerializer.Deserialize<List<LocationCategory>>(@event.Location.Categories),
                EventCategories = JsonSerializer.Deserialize<List<EventCategory>>(@event.Categories),
                EventTickets = tickets
            };

            return View(viewModel);
        }
    }
}
