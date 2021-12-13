using LetsGo.Models;
using LetsGo.Services;
using LetsGo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.Controllers
{
    public class LocationsController : Controller
    {
        private readonly LetsGoContext _db;
        private readonly LocationsService _service;

        public LocationsController(LetsGoContext db, LocationsService service)
        {
            _db = db;
            _service = service;
        }

        public IActionResult Create()
        {
            var categories = _db.LocationCategories.Select(x => new SelectListItem(){ Text = x.Name, Value = x.Id }).ToList();
            var other = categories.FirstOrDefault(l => l.Text == "Другое");
            categories.Remove(other);
            categories.Add(other);
            CreateLocationViewModel model = new CreateLocationViewModel()
            {
                LocationCategories = categories
            };  

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLocationViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _service.Create(model);
                return Json(new { success = true, href = "/Home/Index" });
            }
            return Json(new { success = false });
        }

        public IActionResult GetAllLocations()
        {
            var locations = _db.Locations;
            foreach (var location in locations)
            {
                List<LocationCategory> categories = JsonConvert.DeserializeObject<List<LocationCategory>>(location.Categories);
                location.Categories = categories[0].Name;
            }

            return Json(locations);
        }
        public async Task<IActionResult> Details(string id)
        {
            Location location = await _service.GetLocation(id);
            var events = await _service.GetLocationEvents(id);
            DateTime maxDate = events.Max(p => p.EventStart);
            LocationDetailsViewModel viewModel = new LocationDetailsViewModel
            {
                Location = location,
                LocationCategories = JsonConvert.DeserializeObject<List<LocationCategory>>(location.Categories),
                FutureEvents = events.Where(e => e.EventStart >= DateTime.Now).ToList(),
                PastEvents = events.Where(e => e.EventStart < DateTime.Now).ToList(),
                MaxDate = maxDate
            };
            return View(viewModel);
        }

    }
}