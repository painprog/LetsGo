using System;
using System.Collections.Generic;
using LetsGo.Services;
using LetsGo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using LetsGo.Core.Entities;
using LetsGo.Core.Entities.Enums;
using LetsGo.DAL;
using LetsGo.Extensions;

namespace LetsGo.Controllers
{
    [Authorize]
    public class CabinetController : Controller
    {
        private readonly EventsService _service;
        private readonly CabinetService _cabService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CabinetController(EventsService service, CabinetService cabService,
            ApplicationDbContext context, UserManager<User> userManager)
        {
            _service = service;
            _cabService = cabService;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Profile(Status Status, DateTime DateTimeFrom, DateTime DateTimeBefore, string EventCategs)
        {
            User user = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserIdAsInt(User));
            List<string> EventCategories = new List<string>();
            if (!string.IsNullOrEmpty(EventCategs))
                EventCategories = EventCategs.Split(',').ToList();

            ProfileViewModel viewModel = new ProfileViewModel { User = user };
            Dictionary<string, Status> Stats = _cabService.GetDictionaryStats();
            viewModel.Stats = Stats;
            viewModel.EventCategories = _context.EventCategories.ToList();

            IQueryable<Event> Events = _cabService.QueryableEventsAfterFilter(EventCategories, Status,
                DateTimeFrom, DateTimeBefore);

            if (User.IsInRole("organizer"))
                viewModel.Events = Events.Where(e => e.OrganizerId == user.Id).ToList();
            else
                viewModel.Events = Events.ToList();

            return View(viewModel);
        }
    }
}
