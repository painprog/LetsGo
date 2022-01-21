using System.Linq;
using LetsGo.Core.Entities.Enums;
using LetsGo.DAL;
using LetsGo.UI.ViewModels;
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
            
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Events = _context.Events.Include(e => e.Location)
                    .Where(e => e.Categories.Contains("Концерты") && e.Status == Status.Published).ToList()
            };
            return View("Category", viewModel);
        }

        public IActionResult Performances()
        {
            ViewBag.CategoryName = _localizer["Performances"];
            
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Events = _context.Events.Include(e => e.Location)
                    .Where(e => e.Categories.Contains("Спектакли") && e.Status == Status.Published).ToList()
            };
            return View("Category", viewModel);
        }

        public IActionResult Children()
        {
            ViewBag.CategoryName = "Children";
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Events = _context.Events.Include(e => e.Location)
                    .Where(e => e.Categories.Contains("Детям") && e.Status == Status.Published).ToList()
            };
            return View("Category", viewModel);
        }

        public IActionResult Classic()
        {
            ViewBag.CategoryName = _localizer["Classic"];
            
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Events = _context.Events.Include(e => e.Location)
                    .Where(e => e.Categories.Contains("Классика") && e.Status == Status.Published).ToList()
            };
            return View("Category", viewModel);
        }

        public IActionResult Excursion()
        {
            ViewBag.CategoryName = _localizer["Excursion"];
            
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Events = _context.Events.Include(e => e.Location)
                    .Where(e => e.Categories.Contains("Классика") && e.Status == Status.Published).ToList()
            };
            return View("Category", viewModel);
        }

        public IActionResult Festivals()
        {
            ViewBag.CategoryName = _localizer["Festivals"];
          
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Events = _context.Events.Include(e => e.Location)
                    .Where(e => e.Categories.Contains("Фестивали") && e.Status == Status.Published).ToList()
            };
            return View("Category", viewModel);
        }

        public IActionResult Other()
        {
            ViewBag.CategoryName = _localizer["Other"];
           
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Events = _context.Events.Include(e => e.Location)
                    .Where(e => e.Categories.Contains("Другое") && e.Status == Status.Published).ToList()
            };
            return View("Category", viewModel);
        }
    }
}