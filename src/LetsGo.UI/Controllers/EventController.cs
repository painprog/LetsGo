using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LetsGo.Core.Entities;
using LetsGo.Core.Entities.Enums;
using LetsGo.DAL;
using LetsGo.UI.Extensions;
using LetsGo.UI.Services;
using LetsGo.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LetsGo.UI.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly EventsService _Service;
        private readonly ApplicationDbContext _goContext;
        private readonly UserManager<User> _userManager;

        public EventController(EventsService service, ApplicationDbContext goContext, UserManager<User> userManager)
        {
            _Service = service;
            _goContext = goContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Add()
        {
            var parentCategories = _goContext.EventCategories
                .Where(c => c.HasParent == false)
                .OrderBy(c => c.Id).ToList();

            var childCategories = _goContext.EventCategories
                .Where(c => c.HasParent == true)
                .OrderBy(c => c.ParentId).ToList();

            AddEventViewModel model = new AddEventViewModel()
            {
                ParentCategories = parentCategories,
                ChildCategories = childCategories
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
                model.OrganizerId = _userManager.GetUserIdAsInt(User);
                Event @event = await _Service.AddEvent(model);
                ticketTypes = await _Service.AddEventTicketTypes(@event.Id, ticketTypes);
                return Json(new { success = true, href = "/Home/Index" });
            }
            return Json(new { succes = false });
        }

        public async Task<IActionResult> Edit(int id)
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
                List<EventTicketType> ticketTypes = JsonConvert.DeserializeObject<List<EventTicketType>>(viewModel.Tickets);
                await _Service.UpdateEventTicketTypes(viewModel.Id, ticketTypes);
                Event @event = await _Service.EditEvent(viewModel);
                int[] deletedIds = viewModel.TicketsForDel == null ? null : viewModel.TicketsForDel.Split(',').Select(i => int.Parse(i)).ToArray();
                if (deletedIds != null)
                {
                    await _Service.DeleteEventTicketTypes(deletedIds);
                }
                return Json(new { success = true, href = "/Cabinet/Profile" });
            }
            return Json(new { succes = false });
        }

        public async Task<IActionResult> Details(int id)
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
        public async Task<JsonResult> ChangeStatus(string status, int eventId, string cause)
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

        public void SetMainEventCategory(IEnumerable<Event> events)
        {
            foreach (var item in events)
            {
                item.Categories = JsonConvert.DeserializeObject<List<EventCategory>>(item.Categories)[0].Name;
            }
        }

        public IActionResult Afisha(string SelectedDates, string SelectedCategories)
        {
            List<int> EventCategories = new List<int>();
            if (!string.IsNullOrEmpty(SelectedCategories))
                EventCategories = SelectedCategories.Split(',').Select(c => int.Parse(c)).ToList();

            IQueryable<Event> events = _Service.QueryableEventsAfterFilter(
                   EventCategories, Status.Published, DateTime.MinValue, DateTime.MinValue
            );
            events = _Service.FilterEventsByDate(events, SelectedDates);

            SetMainEventCategory(events);
            ViewBag.PageTitle = $"Афиша Бишкека";
            AfishaViewModel model = new AfishaViewModel()
            {
                Events = events.ToList(),
                CategoriesDictionary = _goContext.EventCategories.ToArray()
                    .GroupBy(c => c.ParentId).ToDictionary(g => g.Key.HasValue ? g.Key : -1, g => g.ToList())
            };
            return View(model);
        }

        // ?
        public int GetEventsQty(string year, string month, string day)
        {
            DateTime date = new DateTime(int.Parse(year), DateTime.ParseExact(month, "MMMM", new CultureInfo("ru-RU")).Month, int.Parse(day));
            int count = _goContext.Events.Where(e => e.EventStart.Date == date.Date).Count();
            return count;
        }

    }
}
