using LetsGo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using LetsGo.Core.Entities;
using LetsGo.Core.Entities.Enums;
using LetsGo.DAL;

namespace LetsGo.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IndexPageViewModel model = new IndexPageViewModel();

            model.Concerts = _db.Events.Include(e => e.Location)
                .OrderByDescending(e => e.CreatedAt)
                .Where(e => e.Categories.Contains("Концерты") && e.Status == Status.Published)
                .Take(6)
                .ToList();

            model.Festivals = _db.Events.Include(e => e.Location)
                .OrderByDescending(e => e.CreatedAt)
                .Where(e => e.Categories.Contains("Фестивали") && e.Status == Status.Published)
                .Take(6)
                .ToList();

            model.Performances = _db.Events.Include(e => e.Location)
                .OrderByDescending(e => e.CreatedAt)
                .Where(e => e.Categories.Contains("Спектакли") && e.Status == Status.Published)
                .Take(6)
                .ToList();

            model.ForChildren = _db.Events.Include(e => e.Location)
                .OrderByDescending(e => e.CreatedAt)
                .Where(e => e.Categories.Contains("Детям") && e.Status == Status.Published)
                .Take(6)
                .ToList();

            model.Locations = _db.Locations.Take(6).ToList();
            foreach(var location in model.Locations)
            {
                List<LocationCategory> categories = JsonConvert.DeserializeObject<List<LocationCategory>>(location.Categories);
                location.Categories = categories[0].Name;
            }
            
            return View(model);
        }
        
        public IActionResult Search(string search)
        {
            IndexPageViewModel ivm = new IndexPageViewModel()
            {
                Concerts = _db.Events.Include(e => e.Location).Where(x => x.Name.Contains(search)).ToList(),
                Locations = _db.Locations.Where(x => x.Name.Contains(search)).ToList()
            };
            return PartialView(ivm);
        }

    }
}