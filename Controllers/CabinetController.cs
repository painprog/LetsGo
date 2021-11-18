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
        private readonly LetsGoContext _context;
        private readonly UserManager<User> _userManager;

        public CabinetController(EventsService service, LetsGoContext context, UserManager<User> userManager)
        {
            _service = service;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Profile()
        {
            User user = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
            ProfileViewModel viewModel = new ProfileViewModel { User = user };

          //  List<Event> events = _context.Events.ToList();
          //  foreach (var item in events)
          //  {
          //      item.Status = Status.Rejected;
          //      _context.Update(item);
          //  }
          //  _context.SaveChanges();


            if (User.IsInRole("organizer")) 
                viewModel.Events = _context.Events.Include(e => e.Location).Where(e => e.OrganizerId == user.Id).ToList();
            else
                viewModel.Events = _context.Events.Include(e => e.Location).Where(e => e.StatusId != (int)Status.Expired).ToList();
            return View(viewModel);
        }  

        //Отправка запроса на снятие с пубилкации мероприятия
        [HttpPost]
        public async Task<JsonResult> RequestForUnPublish(string id)
        {
            Event @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            @event.Status = Status.UnPublished;
            @event.CreatedAt = DateTime.Now;
            _context.Events.Update(@event);
            await _context.SaveChangesAsync();
            string unPublished = @event.Status.ToString();
            return Json(new { unPubl = unPublished, date = @event.CreatedAt });
        }

        //Отправка запроса на возвращение в публикации мероприятия
        [HttpPost]
        public async Task<JsonResult> RequestForPublishAgain(string id)
        {
            Event @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            @event.Status = Status.Published;
            @event.CreatedAt = DateTime.Now;
            _context.Events.Update(@event);
            await _context.SaveChangesAsync();
            string published = @event.Status.ToString();
            return Json(new { publ = published, date = @event.CreatedAt });
        }


        //Отправка запроса на модерацию мероприятия
        [HttpPost]
        public async Task<JsonResult> RequestForNew(string id)
        {
            Event @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            @event.Status = Status.New;
            @event.CreatedAt = DateTime.Now;
            _context.Events.Update(@event);
            await _context.SaveChangesAsync();
            string neW = @event.Status.ToString();
            return Json(new { nEw = neW, date = @event.CreatedAt });
        }

    }
}
