using LetsGo.Models;
using LetsGo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LetsGo.Controllers
{
    public class CategoryController : Controller
    {
        private readonly LetsGoContext _context;

        public CategoryController(LetsGoContext context)
        {
            _context = context;
        }

        public IActionResult Concerts()
        {
            ViewBag.CategoryName = "Концерты";
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Events = _context.Events.Include(e => e.Location).Where(e => e.Categories.Contains("Концерты")).ToList()
            };
            return View("Category", viewModel);
        }

        public IActionResult Performances()
        {
            ViewBag.CategoryName = "Спектакли";
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Events = _context.Events.Include(e => e.Location).Where(e => e.Categories.Contains("Спектакли")).ToList()
            };
            return View("Category", viewModel);
        }

        public IActionResult Children()
        {
            ViewBag.CategoryName = "Детям";
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Events = _context.Events.Include(e => e.Location).Where(e => e.Categories.Contains("Детям")).ToList()
            };
            return View("Category", viewModel);
        }

        public IActionResult Classic()
        {
            ViewBag.CategoryName = "Классика";
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Events = _context.Events.Include(e => e.Location).Where(e => e.Categories.Contains("Классика")).ToList()
            };
            return View("Category", viewModel);
        }

        public IActionResult Excursion()
        {
            ViewBag.CategoryName = "Экскурсии";
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Events = _context.Events.Include(e => e.Location).Where(e => e.Categories.Contains("Классика")).ToList()
            };
            return View("Category", viewModel);
        }

        public IActionResult Festivals()
        {
            ViewBag.CategoryName = "Фестивали";
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Events = _context.Events.Include(e => e.Location).Where(e => e.Categories.Contains("Фестивали")).ToList()
            };
            return View("Category", viewModel);
        }
    }
}
