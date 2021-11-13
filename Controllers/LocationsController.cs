using LetsGo.Models;
using LetsGo.Services;
using LetsGo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

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
                LocationCategories = _db.LocationCategories.Select(x => new SelectListItem()
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
    }
}