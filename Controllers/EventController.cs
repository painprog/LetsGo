using LetsGo.Models;
using LetsGo.Services;
using LetsGo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
                List<EventTicketType> ticketTypes = System.Text.Json.JsonSerializer.Deserialize<List<EventTicketType>>(tickets);
                model.OrganizerId = _userManager.GetUserId(User);
                Event @event = await _Service.AddEvent(model);
                ticketTypes = await _Service.AddEventTicketTypes(@event.Id, ticketTypes);
                return Json(new { success = true, href = "/Home/Index" });
            }
            return Json(new { succes = false });
        }

        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.Locations = await _goContext.Locations.ToListAsync();
            EditEventViewModel viewModel = await _Service.MakeEditEventViewModel(id);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditEventViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                List<EventTicketType> ticketTypes = System.Text.Json.JsonSerializer.Deserialize<List<EventTicketType>>(viewModel.Tickets);
                Event @event = await _Service.EditEvent(viewModel);
                await _Service.UpdateEventTicketTypes(@event.Id, ticketTypes);
                string[] deletedIds = viewModel.TicketsForDel == null ? null : viewModel.TicketsForDel.Split(',');
                if (deletedIds != null)
                {
                    await _Service.DeleteEventTicketTypes(deletedIds);
                }
                return Json(new { success = true, href = "/Cabinet/Profile" });
            }
            return Json(new { succes = false });
        }

        public async Task<IActionResult> Details(string id)
        {
            Event @event = await _Service.GetEvent(id);
            var tickets = _goContext.EventTicketTypes.Where(t => t.EventId == id).ToList();
            DetailsViewModel viewModel = new DetailsViewModel
            {
                Event = @event,
                LocationCategories = JsonConvert.DeserializeObject<List<LocationCategory>>(@event.Location.Categories),
                EventCategories = JsonConvert.DeserializeObject<List<EventCategory>>(@event.Categories),
                EventTickets = tickets
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<JsonResult> ChangeStatus(string status, string eventId, string cause)
        {
            if (await _Service.ChangeStatus(status, eventId, cause))
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }
}
