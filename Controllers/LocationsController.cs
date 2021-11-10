using LetsGo.Models;
using LetsGo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.Controllers
{
    public class LocationsController : Controller
    {
        private readonly LetsGoContext _db;

        public LocationsController(LetsGoContext db)
        {
            _db = db;
        }

        public IActionResult Create()
        {
            var locationCategories = _db.LocationCategories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id
            }).ToList();

            CreateLocationViewModel model = new CreateLocationViewModel()
            {
                LocationCategories = locationCategories
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLocationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var categories = model.LocationCategories.Where(x => x.Selected).Select(x => new
                {
                    Id = x.Value,
                    Name = x.Text
                }).ToList();
                var categoriesJson = JsonConvert.SerializeObject(categories);
                model.Location.Categories = categoriesJson;

                List<string> phones = new List<string>();
                if (model.PhoneNums != null)
                    phones = model.PhoneNums.Split(',').ToList();
                if (model.Location.Phones != null)
                    phones.Add(model.Location.Phones);
                var phoneNumbersJson = JsonConvert.SerializeObject(phones);
                model.Location.Phones = phoneNumbersJson;

                await _db.Locations.AddAsync(model.Location);
                await _db.SaveChangesAsync();

                return Json("success");
            }
            return View(model);
        }
    }
}