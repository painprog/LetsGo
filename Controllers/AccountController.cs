using LetsGo.Enums;
using LetsGo.Models;
using LetsGo.Services;
using LetsGo.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LetsGo.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private LetsGoContext _context;
        private readonly EventsService _Service;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, LetsGoContext context, EventsService service)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _Service = service;
        }

        public IActionResult Profile()
        {
            User user = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
            ProfileViewModel viewModel = new ProfileViewModel { User = user };

            if (User.IsInRole("organizer"))
                viewModel.Events = _context.Events.Include(e => e.Location).Where(e => e.OrganizerId == user.Id).ToList();
            else
                viewModel.Events = _context.Events.Include(e => e.Location).Where(e => e.StatusId != (int)Status.Expired).ToList();

            return View(viewModel);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                User user = await _userManager.FindByEmailAsync(model.LoginOrEmail) ?? await _userManager.FindByNameAsync(model.LoginOrEmail);
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(
                  user,
                  model.Password,
                  model.RememberMe,
                  false
                  );
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
