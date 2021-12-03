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
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using PasswordGenerator;

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
        
        [Authorize(Roles = "superadmin")]
        [HttpGet]
        public IActionResult AddAdmin()
        {
            return View();
        }

        [Authorize(Roles = "superadmin")]
        [HttpPost]
        public async Task<IActionResult> AddAdmin(AddAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                string pathImage;
                if (model.Avatar == null) pathImage = "/images/default_avatar.png";
                else
                {
                    pathImage = "/avatars/" + EventsService.GenerateCode() + Path.GetExtension(model.Avatar.FileName);
                    using (var fileStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\" + pathImage), FileMode.Create))
                        await model.Avatar.CopyToAsync(fileStream);
                }

                User admin = new User
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    PhoneNumber = model.PhoneNumber,
                    AvatarLink = pathImage
                };

                string password = new Password().Next();
                var result = await _userManager.CreateAsync(admin, password);

                if (result.Succeeded)
                {
                    EmailService emailService = new EmailService();
                    await emailService.Send(
                        admin.Email,
                        "Логин и пароль от аккаунта админа",
                        $"Здравствуйте! Вот ваши данные для входа в аккаунт. Никому их не передавайте <br />    Login: {admin.UserName}<br />    Email: {admin.Email}<br />    Password: {password}"
                    );
                    await _userManager.AddToRoleAsync(admin, "admin");
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return RedirectToAction("Index", "Home");
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
                    string pathImage = string.Empty;
                    if (model.Avatar is null)
                        pathImage = "/images/default_avatar.png";
                    else
                    {
                        string name = EventsService.GenerateCode() + Path.GetExtension(model.Avatar.FileName);
                        pathImage = "/avatars/" + name;
                        using (var fileStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\" + pathImage), FileMode.Create))
                            await model.Avatar.CopyToAsync(fileStream);
                    }

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

        // Validations
        public bool CheckEmail(string email)
        {
            return !_userManager.Users.Any(b => b.Email == email);
        }

        public bool CheckUserName(string userName)
        {
            return !_userManager.Users.Any(b => b.UserName == userName);
        }

    }
}
