using LetsGo.Models;
using LetsGo.Services;
using LetsGo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Globalization;

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
            var categories = _goContext.EventCategories.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id }).ToList();
            var other = categories.FirstOrDefault(l => l.Text == "Другое");
            categories.Remove(other);
            categories.Add(other);
            AddEventViewModel model = new AddEventViewModel()
            {
                Categories = categories
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

        public void SetMainEventCategory(List<Event> events)
        {
            foreach (var item in events)
            {
                item.Categories = JsonConvert.DeserializeObject<List<EventCategory>>(item.Categories)[0].Name;
            }
        }

        public IActionResult Afisha()
        {
            var events = _goContext.Events.Include(e => e.Location)
                .Where(e => e.EventStart.Month == DateTime.Now.Month).OrderBy(e => e.EventStart).ToList();
            SetMainEventCategory(events);
            ViewBag.PageTitle = $"Афиша Бишкека на {DateTime.Now.ToString("MMMM", new System.Globalization.CultureInfo("ru-RU")).ToLower()} {DateTime.Now.Year}";
            return View(events);
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
