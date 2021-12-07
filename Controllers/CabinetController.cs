﻿using LetsGo.Enums;
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

        public IActionResult Profile(string EventCategory, Status Status, DateTime DateTimeFrom, DateTime DateTimeBefore)
        {
            User user = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
            ProfileViewModel viewModel = new ProfileViewModel { User = user };   
            Dictionary<string, Status> Stats = _cabService.GetDictionaryStats();          
            viewModel.Stats = Stats;
            viewModel.EventCategories = _context.EventCategories.ToList();

            IQueryable<Event> Events = _cabService.QueryableEventsAfterFilter(EventCategory, Status,
                DateTimeFrom, DateTimeBefore);

            if (User.IsInRole("organizer"))
                viewModel.Events = Events.Where(e => e.OrganizerId == user.Id).ToList();
            else
                viewModel.Events = Events.ToList();

            return View(viewModel);
        }
    }
}
