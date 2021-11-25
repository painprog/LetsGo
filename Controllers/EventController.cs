using LetsGo.Models;
using LetsGo.Services;
using LetsGo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly UserManager<User> _userManager;

        public EventController(EventsService service, LetsGoContext goContext, UserManager<User> userManager)
        {
            _Service = service;
            _goContext = goContext;
            _userManager = userManager;
        }


        public async Task<IActionResult> Add()
        {
            AddEventViewModel model = new AddEventViewModel()
            {
                Categories = _goContext.EventCategories.Where(c => c.Name != "Другое")
                .Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id
                }).ToList(),
                EventStart = null,
                EventEnd = null
            };

            ViewBag.Locations = await _goContext.Locations.ToListAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> Add(AddEventViewModel model)
        {
            if (ModelState.IsValid)
            {
                string tickets = model.Tickets;
                List<EventTicketType> ticketTypes = JsonSerializer.Deserialize<List<EventTicketType>>(tickets);
                model.OrganizerId = _userManager.GetUserId(User);
                Event @event = await _Service.AddEvent(model);
                ticketTypes = await _Service.AddEventTicketTypes(@event.Id, ticketTypes);
                return Json(new { success = true, href = "/Home/Index" });
            }
            return Json(new { succes = false });
        }
        public async Task<IActionResult> Details(string id)
        {
            Event @event = _Service.GetEvent(id).Result;
            var tickets = _goContext.EventTicketTypes.Where(t => t.EventId == id).ToList();
            DetailsViewModel viewModel = new DetailsViewModel
            {
                Event = @event,
                LocationCategories = JsonSerializer.Deserialize<List<LocationCategory>>(@event.Location.Categories),
                EventCategories = JsonSerializer.Deserialize<List<EventCategory>>(@event.Categories),
                EventTickets = tickets
            };
            return View(viewModel);
        }
    }
}
