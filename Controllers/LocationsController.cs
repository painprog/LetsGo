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
            CreateLocationViewModel model = new CreateLocationViewModel()
            {
                LocationCategories = _db.LocationCategories.Where(c => c.Name != "Другое").Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CreateLocationViewModel model)
        {
            if (ModelState.IsValid)
            {
                _service.Create(model);
                return Json("success");
            }
            return View(model);
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
            LocationDetailsViewModel viewModel = new LocationDetailsViewModel
            {
                Location = location,
                FutureEvents = events.Where(e => e.EventStart >= DateTime.Now).ToList(),
                PastEvents = events.Where(e => e.EventStart >= DateTime.Now).ToList()
            };
            return View(viewModel);
        }

    }
}