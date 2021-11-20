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
            if (!_goContext.EventCategories.Any())
            {
                EventCategory categoryPlay = new EventCategory { Name = "Спектакль" };
                EventCategory categoryShow = new EventCategory { Name = "Шоу" };
                EventCategory categoryConcert = new EventCategory { Name = "Концерт" };
                EventCategory categoryFreekShow = new EventCategory { Name = "Фрик-шоу" };
                await _goContext.EventCategories.AddAsync(categoryPlay);
                await _goContext.EventCategories.AddAsync(categoryShow);
                await _goContext.EventCategories.AddAsync(categoryConcert);
                await _goContext.EventCategories.AddAsync(categoryFreekShow);
            }
            await _goContext.SaveChangesAsync();
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

        public async Task<IActionResult> Edit(string id)
        {
            List<EventCategory> categories = await _goContext.EventCategories.ToListAsync();
            ViewBag.Categories = categories;
            ViewBag.Locations = await _goContext.Locations.ToListAsync();
            EditEventViewModel viewModel = await _Service.MakeEditEventViewModel(id);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditEventViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                List<EventTicketType> ticketTypes = JsonSerializer.Deserialize<List<EventTicketType>>(viewModel.Tickets);
                Event @event = await _Service.EditEvent(viewModel);
                await _Service.AddEventTicketTypes(@event.Id, ticketTypes);
                if (viewModel.TicketsForDel != null)
                {
                    List<EventTicketType> ticketTypesForDel = JsonSerializer.Deserialize<List<EventTicketType>>(viewModel.TicketsForDel);
                    await _Service.DeleteEventTicketTypes(@event.Id, ticketTypesForDel);
                }
                return Json(new { success = true, href = "/Home/Index" });
            }
            return Json(new { succes = false });
        }
    }
}
