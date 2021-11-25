using LetsGo.Models;
using LetsGo.Services;
using LetsGo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
            ViewBag.Locations = await _goContext.Locations.ToListAsync();
            return View();
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
       
        
        public  IActionResult Details(string id = "697c34b6-3607-49fc-9c89-307f615e8931")
        {
            var @event = _Service.GetEvent(id).Result;
            var tickets =  _goContext.EventTicketTypes.Where(t => t.EventId == id).ToList();
            var viewModel = new DetailsViewModel
            {
                Event = @event,
                LocationCategories = JsonConvert.DeserializeObject<List<LocationCategory>>(@event.Location.Categories),
                EventTickets = tickets
            };
            return View(viewModel);
        }
        
        
    }
}
