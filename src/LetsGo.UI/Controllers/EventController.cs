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

        public IActionResult Afisha(Status Status, DateTime DateTimeFrom, DateTime DateTimeBefore, string selectedCategories)
        {
            List<int> EventCategories = new List<int>();
            if (!string.IsNullOrEmpty(selectedCategories))
                EventCategories = selectedCategories.Split(',').Select(c => int.Parse(c)).ToList();

            IQueryable<Event> events = _Service.QueryableEventsAfterFilter(
                   EventCategories, Status, DateTimeFrom, DateTimeBefore
               );

            //var events = _goContext.Events.Include(e => e.Location)
            //    .Where(e => e.EventStart.Month == DateTime.Now.Month).OrderBy(e => e.EventStart).ToList();
            SetMainEventCategory(events);
            ViewBag.PageTitle = $"Афиша Бишкека на {DateTime.Now.ToString("MMMM", new System.Globalization.CultureInfo("ru-RU")).ToLower()} {DateTime.Now.Year}";
            AfishaViewModel model = new AfishaViewModel()
            {
                Events = events.ToList(),
                CategoriesDictionary = _goContext.EventCategories.ToArray()
                    .GroupBy(c => c.ParentId).ToDictionary(g => g.Key.HasValue ? g.Key : -1, g => g.ToList())
            };
            return View(model);
        }

        public IActionResult AfishaOn(string year, string month, string day)
        {
            DateTime date = new DateTime(int.Parse(year),
                DateTime.ParseExact(month, "MMMM", new CultureInfo("ru-RU")).Month,
                int.Parse(day));

            var events = _goContext.Events.Include(e => e.Location).Where(e => e.EventStart.Date == date.Date).OrderBy(e => e.EventStart).ToList();
            SetMainEventCategory(events);

            ViewBag.PageTitle = $"Афиша Бишкека {date.ToString("d MMMM", new System.Globalization.CultureInfo("ru-RU")).ToLower()}";
            return View("Afisha", events);
        }

        public int GetEventsQty(string year, string month, string day)
        {
            DateTime date = new DateTime(int.Parse(year), DateTime.ParseExact(month, "MMMM", new CultureInfo("ru-RU")).Month, int.Parse(day));
            int count = _goContext.Events.Where(e => e.EventStart.Date == date.Date).Count();
            return count;
        }

        public IActionResult Today()
        {
            var events = _goContext.Events.Include(e => e.Location)
                .Where(e => e.EventStart == DateTime.Now).OrderBy(e => e.EventStart).ToList();
            SetMainEventCategory(events);
            ViewBag.PageTitle = $"Афиша Бишкека {DateTime.Now.ToString("d MMMM", new System.Globalization.CultureInfo("ru-RU")).ToLower()}";
            return View("Afisha", events);
        }

        public IActionResult Tomorrow()
        {
            var tomorrow = DateTime.Now.AddDays(1);
            var events = _goContext.Events.Include(e => e.Location)
                .Where(e => e.EventStart == tomorrow).OrderBy(e => e.EventStart).ToList();
            SetMainEventCategory(events);
            ViewBag.PageTitle = $"Афиша Бишкека {tomorrow.ToString("d MMMM", new System.Globalization.CultureInfo("ru-RU")).ToLower()}";
            return View("Afisha", events);
        }

        public IActionResult Weekends()
        {
            DateTime sunday;
            var firstDayOfWeek = new CultureInfo("ky-KG").DateTimeFormat.FirstDayOfWeek;
            int offset = firstDayOfWeek - DateTime.Now.DayOfWeek;
            if (offset != 1)
            {
                DateTime weekStart = DateTime.Now.AddDays(offset);
                DateTime endOfWeek = weekStart.AddDays(6);
                sunday = endOfWeek;
            }
            else
                sunday = DateTime.Now;
            
            var events = _goContext.Events.Include(e => e.Location).OrderBy(e => e.EventStart)
                .Where(e => e.EventStart.Date <= sunday.Date || e.EventStart.Date <= sunday.AddDays(-1).Date).ToList();

            foreach (var item in new List<Event>(events))
                if (!(item.EventStart.DayOfWeek == DayOfWeek.Saturday || item.EventStart.DayOfWeek == DayOfWeek.Sunday))
                    events.Remove(item);
            
            SetMainEventCategory(events);
            ViewBag.PageTitle = $"Афиша Бишкека на " + sunday.AddDays(-1).Day +
                " и " + sunday.ToString("d MMMM", new System.Globalization.CultureInfo("ru-RU"));
            return View("Afisha", events);
        }
    }
}
