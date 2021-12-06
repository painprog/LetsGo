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

        public IActionResult Profile()
        {
            User user = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
           //IQueryable Events = _context.Events.Include(e => e.Location).OrderBy(e => e.Status).ThenByDescending(e => e.CreatedAt);
           //if (true)
           //{
           //
           //}
            ProfileViewModel viewModel = new ProfileViewModel { User = user };
            viewModel.EventCategories = _context.EventCategories.ToList();

            if (User.IsInRole("organizer"))
                viewModel.Events = _context.Events.Include(e => e.Location).Where(e => e.OrganizerId == user.Id).ToList();
            else
                viewModel.Events = _context.Events.Include(e => e.Location).OrderBy(e => e.Status).ThenByDescending(e => e.CreatedAt).ToList();

            return View(viewModel);
        }
    }
}
