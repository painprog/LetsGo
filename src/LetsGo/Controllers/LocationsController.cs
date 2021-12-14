using LetsGo.Services;
using LetsGo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using LetsGo.Core.Entities;
using LetsGo.DAL;

namespace LetsGo.Controllers
{
    public class LocationsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly LocationsService _service;

        public LocationsController(ApplicationDbContext db, LocationsService service)
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
                    Value = x.Id.ToString()
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
    }
}