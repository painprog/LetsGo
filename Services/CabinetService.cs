using LetsGo.Enums;
using LetsGo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.Services
{
    public class CabinetService
    {
        private readonly EventsService _service;
        private readonly LetsGoContext _context;
        private readonly UserManager<User> _userManager;

        public CabinetService(EventsService service, LetsGoContext context, UserManager<User> userManager)
        {
            _service = service;
            _context = context;
            _userManager = userManager;
        }

        public async Task<Event> ChangeStatus(string id, Status status)
        {
            Event @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            @event.Status = status;
            @event.CreatedAt = DateTime.Now;
            _context.Events.Update(@event);
            await _context.SaveChangesAsync();
            return @event;
        }
    }
}
