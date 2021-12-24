using LetsGo.Enums;
using LetsGo.Models;
using LetsGo.Services;
using LetsGo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.Controllers
{
    [Authorize]
    public class CabinetController : Controller
    {
        private readonly EventsService _service;
        private readonly CabinetService _cabService;
        private readonly LetsGoContext _context;
        private readonly UserManager<User> _userManager;

        public CabinetController(EventsService service, CabinetService cabService,
            LetsGoContext context, UserManager<User> userManager)
        {
            _service = service;
            _cabService = cabService;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Profile(Status Status, DateTime DateTimeFrom, DateTime DateTimeBefore, string EventCategs, SortState SortOrder = SortState.DateStartDesc)
        {
            List<string> EventCategories = new List<string>();
            if (!string.IsNullOrEmpty(EventCategs))
                EventCategories = EventCategs.Split(',').ToList();

            User user = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
            ProfileViewModel viewModel = new ProfileViewModel { User = user };
            Dictionary<string, Status> Stats = _cabService.GetDictionaryStats();
            viewModel.Stats = Stats;
            viewModel.EventCategories = _context.EventCategories.ToList();

            IQueryable<Event> Events = _cabService.QueryableEventsAfterFilter(EventCategories, Status,
                DateTimeFrom, DateTimeBefore);

            if (User.IsInRole("organizer"))
                Events = Events.Where(e => e.OrganizerId == user.Id);

            ViewBag.NameSort = SortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewBag.PriceSort = SortOrder == SortState.PriceAsc ? SortState.PriceDesc : SortState.PriceAsc;
            ViewBag.DateStartSort = SortOrder == SortState.DateStartAsc ? SortState.DateStartDesc : SortState.DateStartAsc;
            switch (SortOrder)
            {
                case SortState.NameDesc:
                    Events = Events.OrderByDescending(s => s.Name);
                    break;
                case SortState.PriceAsc:
                    Events = Events.OrderBy(s => s.MinPrice);
                    break;
                case SortState.PriceDesc:
                    Events = Events.OrderByDescending(s => s.MinPrice);
                    break;
                case SortState.DateStartAsc:
                    Events = Events.OrderBy(s => s.EventStart);
                    break;
                case SortState.DateStartDesc:
                    Events = Events.OrderByDescending(s => s.EventStart);
                    break;
                default:
                    Events = Events.OrderBy(s => s.Name);
                    break;
            }
            viewModel.Events = Events.ToList();
            return View(viewModel);
        }
    }
}
