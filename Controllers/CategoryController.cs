using LetsGo.Models;
using LetsGo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
