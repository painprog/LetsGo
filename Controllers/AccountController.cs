using LetsGo.Models;
using LetsGo.Services;
using LetsGo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PasswordGenerator;

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
            return RedirectToAction("Index");
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
