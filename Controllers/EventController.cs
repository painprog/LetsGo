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
            List<string> categoriesName = new List<string>();
            List<EventCategory> eventCategories = await _goContext.EventCategories.ToListAsync();
            foreach (var item in eventCategories)
                categoriesName.Add(item.Name);
            ViewBag.Categories = categoriesName;
            ViewBag.AgeLimits = new int[] { 0, 5, 6, 12, 16, 18 };
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Add([FromForm] EventViewModel eventView)
        {     
            Event @event = await _Service.AddEvent(eventView);
            return Json(new { @event });
        }
    }
}
