using System.Collections.Generic;
using System.Linq;
using LetsGo.Core.Entities;
using LetsGo.Core.Entities.Enums;
using LetsGo.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace LetsGo.UI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IStringLocalizer<CategoryController> _localizer;
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context, IStringLocalizer<CategoryController> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public IActionResult Concerts()
        {
            ViewBag.CategoryName = _localizer["Concerts"];
            List<Event> events = _context.Events.Include(e => e.Location)
                .Where(e => e.Categories.Contains("Concerts") && e.Status == Status.Published).ToList();
            return View("Category", events);
        }

        public IActionResult Performances()
        {
            ViewBag.CategoryName = _localizer["Performances"];
            List<Event> events = _context.Events.Include(e => e.Location)
                    .Where(e => e.Categories.Contains("Performances") && e.Status == Status.Published).ToList();
            return View("Category", events);
        }

        public IActionResult Children()
        {
            ViewBag.CategoryName = _localizer["Children"];
            List<Event> events = _context.Events.Include(e => e.Location)
                    .Where(e => e.Categories.Contains("Children") && e.Status == Status.Published).ToList();
            return View("Category", events);
        }

        public IActionResult Classic()
        {
            ViewBag.CategoryName = _localizer["Classic"];
            List<Event> events = _context.Events.Include(e => e.Location)
                    .Where(e => e.Categories.Contains("Classic") && e.Status == Status.Published).ToList();
            return View("Category", events);
        }

        public IActionResult Excursion()
        {
            ViewBag.CategoryName = _localizer["Excursion"];
            List<Event> events = _context.Events.Include(e => e.Location)
                    .Where(e => e.Categories.Contains("Excursion") && e.Status == Status.Published).ToList();
            return View("Category", events);
        }

        public IActionResult Festivals()
        {
            ViewBag.CategoryName = _localizer["Festivals"];
            List<Event> events = _context.Events.Include(e => e.Location)
                    .Where(e => e.Categories.Contains("Festivals") && e.Status == Status.Published).ToList();
            return View("Category", events);
        }

        public IActionResult Other()
        {
            ViewBag.CategoryName = _localizer["Other"];
            List<Event> events = _context.Events.Include(e => e.Location)
                    .Where(e => e.Categories.Contains("Other") && e.Status == Status.Published).ToList();
            return View("Category", events);
        }
    }
}