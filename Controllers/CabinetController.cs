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

        public IActionResult Profile(Status Status, DateTime DateTimeFrom, DateTime DateTimeBefore, string EventCategs)
        {
            List<string> EventCategories = new List<string>();
            if (!string.IsNullOrEmpty(EventCategs))
                EventCategories = EventCategs.Split(',').ToList();

            User user = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
            ProfileViewModel viewModel = new ProfileViewModel {User = user};
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

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            var model = new EditProfileViewModel()
            {
                Id = id,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditProfileViewModel userModel)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == userModel.Id);
            if (currentUser == null) return RedirectToAction("Profile");
            string avatar;
            if (userModel.Avatar == null)
            {
                avatar = currentUser.AvatarLink;
            }
            else
            {
                avatar = "/avatars/" + EventsService.GenerateCode() + Path.GetExtension(userModel.Avatar.FileName);
                using (var fileStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\" + avatar), FileMode.Create))
                    await userModel.Avatar.CopyToAsync(fileStream);
            }
            bool newEmail = currentUser.Email != userModel.Email;
            if (newEmail) currentUser.EmailConfirmed = false;

            currentUser.Email = userModel.Email;
            currentUser.UserName = userModel.UserName;
            currentUser.PhoneNumber = userModel.PhoneNumber;
            currentUser.AvatarLink = avatar;
            _context.Users.Update(currentUser);
            await _context.SaveChangesAsync();
            if (newEmail)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(currentUser);
                await SendConfirmEmail(currentUser, code);
                // выходить из аккаунта
                //return RedirectToAction("Account", "Logout", null);
            }
            return RedirectToAction("Profile");
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
    }
}