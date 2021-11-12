using LetsGo.Models;
using LetsGo.Services;
using LetsGo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LetsGo.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly EventsService _Service;
        private readonly LetsGoContext _goContext;

        public EventController(EventsService service, LetsGoContext goContext)
        {
            _Service = service;
            _goContext = goContext;
        }


        public async Task<IActionResult> Add()
        {
            if (!_goContext.EventCategories.Any())
            {
                EventCategory categoryPlay = new EventCategory { Name = "Спектакль" };
                EventCategory categoryShow = new EventCategory { Name = "Шоу" };
                await _goContext.EventCategories.AddAsync(categoryPlay);
                await _goContext.EventCategories.AddAsync(categoryShow);
                await _goContext.SaveChangesAsync();
            }
            List<EventCategory> categories = await _goContext.EventCategories.ToListAsync();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Add(AddEventViewModel model)
        {
            if (ModelState.IsValid)
            {
                string tickets = model.Tickets;
                List<EventTicketType> ticketTypes = JsonSerializer.Deserialize<List<EventTicketType>>(tickets);
                Event @event = await _Service.AddEvent(model);
                ticketTypes = await _Service.AddEventTicketTypes(@event.Id, ticketTypes);
                return Json(new { success = true });
            }
            return Json(new { succes = false });
        }
    }
}
