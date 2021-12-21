using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using LetsGo.Core.Entities;
using LetsGo.Core.Entities.Enums;
using LetsGo.DAL;
using LetsGo.UI.Extensions;
using LetsGo.UI.Services;
using LetsGo.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LetsGo.UI.Controllers
{
    [Authorize]
    public class CabinetController : Controller
    {
        private readonly EventsService _eventService;
        private readonly CabinetService _cabinetService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer<CabinetController> _localizer;

        public CabinetController(EventsService eventService, CabinetService cabinetService, ApplicationDbContext context, 
                                   UserManager<User> userManager, IStringLocalizer<CabinetController> localizer)	
        {
            _eventService = eventService;
            _cabinetService = cabinetService;
            _context = context;
            _userManager = userManager;
            _localizer = localizer;
        }

        public IActionResult Profile(Status Status, DateTime DateTimeFrom, DateTime DateTimeBefore, string EventCategs)
        {
            User user = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserIdAsInt(User));
            List<string> EventCategories = new List<string>();
            if (!string.IsNullOrEmpty(EventCategs))
                EventCategories = EventCategs.Split(',').ToList();

            ProfileViewModel viewModel = new ProfileViewModel { User = user, IsOrganizer = User.IsInRole("organizer") };
            Dictionary<string, Status> Stats = _cabinetService.GetDictionaryStats();
            viewModel.Stats = Stats;
            viewModel.EventCategories = _context.EventCategories.ToList();

            IQueryable<Event> Events = _cabinetService.QueryableEventsAfterFilter(EventCategories, Status,
                DateTimeFrom, DateTimeBefore);

            if (viewModel.IsOrganizer)
                viewModel.Events = Events.Where(e => e.OrganizerId == user.Id).ToList();
            else
                viewModel.Events = Events.ToList();

            ViewData["Status"] = _localizer["Status"];
            ViewData["NotDefined"] = _localizer["NotDefined"];
            ViewData["New"] = _localizer["New"];
            ViewData["Rejected"] = _localizer["Rejected"];
            ViewData["Published"] = _localizer["Published"];
            ViewData["UnPublished"] = _localizer["UnPublished"];
            ViewData["Edited"] = _localizer["Edited"];
            ViewData["Expired"] = _localizer["Expired"];
            ViewData["ReviewPublished"] = _localizer["ReviewPublished"];
            ViewData["ReviewUnPublished"] = _localizer["ReviewUnPublished"];

            return View(viewModel);
        }
    }
}
