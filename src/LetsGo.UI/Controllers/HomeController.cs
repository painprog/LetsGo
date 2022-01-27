using System;
using System.Collections.Generic;
using System.Linq;
using LetsGo.Core.Entities;
using LetsGo.Core.Entities.Enums;
using LetsGo.DAL;
using LetsGo.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LetsGo.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IQueryable<Event> events = _db.Events.Include(e => e.Location);
            IndexPageViewModel model = new IndexPageViewModel();

            model.Concerts = events
                .OrderByDescending(e => e.EventStart)
                .Where(e => e.Categories.Contains("Concerts") && e.Status == Status.Published)
                .Take(6)
                .ToList();

            model.Festivals = events
                .OrderByDescending(e => e.EventStart)
                .Where(e => e.Categories.Contains("Festivals") && e.Status == Status.Published)
                .Take(6)
                .ToList();

            model.Performances = events
                .OrderByDescending(e => e.EventStart)
                .Where(e => e.Categories.Contains("Perfomances") && e.Status == Status.Published)
                .Take(6)
                .ToList();

            model.ForChildren = events
                .OrderByDescending(e => e.EventStart)
                .Where(e => e.Categories.Contains("ForChildren") && e.Status == Status.Published)
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
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}