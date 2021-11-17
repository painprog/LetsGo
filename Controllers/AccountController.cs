﻿using LetsGo.Models;
using LetsGo.Services;
using LetsGo.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace LetsGo.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private LetsGoContext _context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, LetsGoContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
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

        public IActionResult OrganizerSignUp() => View();

        [HttpPost]
        public async Task<IActionResult> OrganizerSignUp(OrganizerSignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userEmailCheck = await _userManager.FindByEmailAsync(model.Email);
                if (userEmailCheck == null)
                {
                    string name = EventsService.GenerateCode() + '.' + Path.GetExtension(model.Avatar.FileName);
                    string pathImage = "/avatars/" + name;
                    using (var fileStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\" + pathImage), FileMode.Create))
                        await model.Avatar.CopyToAsync(fileStream);

                    User user = new User
                    {
                        UserName = model.Username,
                        Email = model.Email,
                        AvatarLink = pathImage,
                        PhoneNumber = model.PhoneNumber,
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "organizer");
                        await _signInManager.SignInAsync(user, false);
                        return RedirectToAction("Index", "Home");
                    }

                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Пользователь с таким электронным адресом уже зарегистрирован.");
                    return View(model);
                }
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
