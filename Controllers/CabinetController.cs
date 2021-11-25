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
            ProfileViewModel viewModel = new ProfileViewModel { User = user };

            if (User.IsInRole("organizer"))
                viewModel.Events = _context.Events.Include(e => e.Location).Where(e => e.OrganizerId == user.Id).ToList();
            else
                viewModel.Events = _context.Events.Include(e => e.Location).OrderBy(e => e.Status).ThenByDescending(e => e.CreatedAt).ToList();

            return View(viewModel);
        }
        //Отправка запроса на снятие с пубилкации мероприятия
        [HttpPost]
        public async Task<JsonResult> RequestForUnPublish(string id)
        {
            Event @event = await _cabService.ChangeStatus(id, Status.UnPublished);
            return Json(new { unPubl = "Снято с публикации"});
        }

        //Отправка запроса на возвращение в публикации мероприятия
        [HttpPost]
        public async Task<JsonResult> RequestForPublishAgain(string id)
        {
            Event @event = await _cabService.ChangeStatus(id, Status.Published);
            return Json(new { publ = "Возвращено в публикации"});
        }


        //Отправка запроса на модерацию мероприятия
        [HttpPost]
        public async Task<JsonResult> RequestForNew(string id)
        {
            Event @event = await _cabService.ChangeStatus(id, Status.New);
            return Json(new { nEw = "Отправлено на модерацию"});
        }

    }
}
