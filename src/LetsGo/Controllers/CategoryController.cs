using LetsGo.Enums;
using LetsGo.Models;
using LetsGo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using LetsGo.DAL;

namespace LetsGo.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Concerts()
        {
            ViewBag.CategoryName = "Концерты";
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Events = _context.Events.Include(e => e.Location).Where(e => e.Categories.Contains("Концерты") && e.Status == Status.Published).ToList()
            };
            return View("Category", viewModel);
        }

        public IActionResult Performances()
        {
            ViewBag.CategoryName = "Спектакли";
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Events = _context.Events.Include(e => e.Location).Where(e => e.Categories.Contains("Спектакли") && e.Status == Status.Published).ToList()
            };
            return View("Category", viewModel);
        }

        public IActionResult Children()
        {
            ViewBag.CategoryName = "Детям";
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Events = _context.Events.Include(e => e.Location).Where(e => e.Categories.Contains("Детям") && e.Status == Status.Published).ToList()
            };
            return View("Category", viewModel);
        }

        public IActionResult Classic()
        {
            ViewBag.CategoryName = "Классика";
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Events = _context.Events.Include(e => e.Location).Where(e => e.Categories.Contains("Классика") && e.Status == Status.Published).ToList()
            };
            return View("Category", viewModel);
        }

        public IActionResult Excursion()
        {
            ViewBag.CategoryName = "Экскурсии";
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Events = _context.Events.Include(e => e.Location).Where(e => e.Categories.Contains("Классика") && e.Status == Status.Published).ToList()
            };
            return View("Category", viewModel);
        }

        public IActionResult Festivals()
        {
            ViewBag.CategoryName = "Фестивали";
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Events = _context.Events.Include(e => e.Location).Where(e => e.Categories.Contains("Фестивали") && e.Status == Status.Published).ToList()
            };
            return View("Category", viewModel);
        }

        public IActionResult Other()
        {
            ViewBag.CategoryName = "Другое";
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Events = _context.Events.Include(e => e.Location).Where(e => e.Categories.Contains("Другое") && e.Status == Status.Published).ToList()
            };
            return View("Category", viewModel);
        }
    }
}
