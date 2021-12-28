using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LetsGo.Core.Entities;
using LetsGo.DAL;
using LetsGo.UI.Extensions;
using LetsGo.UI.Services;
using LetsGo.UI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PasswordGenerator;

namespace LetsGo.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private ApplicationDbContext _context;
        private readonly EventsService _Service;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context, EventsService service)
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
                var user = await _userManager.FindByEmailAsync(model.LoginOrEmail) ?? await _userManager.FindByNameAsync(model.LoginOrEmail);

                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError(string.Empty, "Вы не подтвердили свой email");
                    return View(model);
                }

                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(
                  user,
                  model.Password,
                  model.RememberMe,
                  false
                  );
                if (result.Succeeded) 
                    return RedirectToAction("Index", "Home");
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
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(admin);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = admin.Id, code = code },
                        protocol: HttpContext.Request.Scheme);
                    await EmailService.Send(
                        admin.Email,
                        "Логин и пароль от аккаунта админа",
                        $"Здравствуйте! Вот ваши данные для входа в аккаунт. Никому их не передавайте <br />    Login: {admin.UserName}<br />    Email: {admin.Email}<br />    Password: {password}<br />" +
                        $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>ссылка</a>"
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

        public IActionResult EmailConfirmForAdmin(int id)
        {
            ViewBag.id = id;
            return View();
        }
             
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
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        await SendConfirmEmail(user, code);

                        await _userManager.AddToRoleAsync(user, "organizer");

                        return View("ConfirmRegistration");
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            var userRoles = await _userManager.GetRolesAsync(user);
            if (result.Succeeded && userRoles.Contains("admin"))
            {
                return RedirectToAction("EmailConfirmForAdmin", "Account", new { userId = user.Id});
            }
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> RedirectToForgotPassword(string loginOrEmail)
        {
            User user = await _userManager.FindByEmailAsync(loginOrEmail) ??
                await _userManager.FindByNameAsync(loginOrEmail);
            if (user != null)
                return Json(new { success = true, email = user.Email });
            else
                return Json(new { success = false });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword(string email) =>
            View(new ForgotPasswordViewModel { Email = email });

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                    return View("ForgotPasswordConfirmation");

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { code = code, email = user.Email },
                    protocol: HttpContext.Request.Scheme);
                await EmailService.Send(model.Email, "Восстановление пароля",
                    $"Для восстановления пароля пройдите по ссылке: <a href='{callbackUrl}'>ссылка</a>");
                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code, string email)
        {
            return code == null ? View("Error") : View(new ResetPasswordViewModel { Code = code, Email = email });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            var user = await _userManager.FindByEmailAsync(model.Email);

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return View("ResetPasswordConfirmation");
            }
            if(result.Errors.FirstOrDefault(e => e.Code == "InvalidToken") != null)
            {
                ModelState.AddModelError("", "Это не ваш почтовый адрес");
                return View(model);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }


        public async Task SendConfirmEmail(User user, string code)
        {
            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                new { userId = user.Id, code = code },
                protocol: HttpContext.Request.Scheme);
            await EmailService.Send(user.Email, "Подтвердите ваш аккаунт",
                $"Подтвердите регистрацию, перейдя по ссылке:" +
                $" <a href='{callbackUrl}'>ссылка</a>");
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

        public bool CheckEmailEdit(string email)
        {
            if (_userManager.Users.FirstOrDefault(u => u.Id == _userManager.GetUserIdAsInt(User)).Email == email) return true;
            return !_userManager.Users.Any(b => b.Email == email);
        }

        public bool CheckUserNameEdit(string userName)
        {
            if (_userManager.Users.FirstOrDefault(u => u.Id == _userManager.GetUserIdAsInt(User)).Email == userName) return true;
            return !_userManager.Users.Any(b => b.Email == userName);
        }


        public async Task<JsonResult> LoginChek(string loginOrEmail)
        {
            var user = await _userManager.FindByEmailAsync(loginOrEmail) ?? await _userManager.FindByNameAsync(loginOrEmail);
            return Json(user != null);
        }

        public async Task<JsonResult> EmailChek(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return Json(user != null);
        }
    }
}
