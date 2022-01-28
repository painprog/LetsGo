using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetsGo.Core.Entities;
using LetsGo.Core.Entities.Enums;
using LetsGo.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LetsGo.UI.Services
{
    public class CabinetService
    {
        private readonly EventsService _service;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CabinetService(EventsService service, ApplicationDbContext context, UserManager<User> userManager)
        {
            _service = service;
            _context = context;
            _userManager = userManager;
        }

        public async Task<Event> ChangeStatus(int id, Status status)
        {
            Event @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
            @event.Status = status;
            @event.CreatedAt = DateTime.Now;
            _context.Events.Update(@event);
            await _context.SaveChangesAsync();
            return @event;
        }

        public Dictionary<string, Status> GetDictionaryStats()
        {
            Dictionary<string, Status> Stats = new Dictionary<string, Status>
            {
                ["Не определено"] = Status.NotDefined,
                ["Новое"] = Status.New,
                ["Отклонено"] = Status.Rejected,
                ["Опубликовано"] = Status.Published,
                ["Не опубликовано"] = Status.UnPublished,
                ["Отредактировано"] = Status.Edited,
                ["Истекло"] = Status.Expired,
                ["Ожидание добавления"] = Status.ReviewPublished,
                ["Ожидание снятия"] = Status.ReviewUnPublished,
            };
            return Stats;
        }

        public IQueryable<Event> GetEvents()
        {
            IQueryable<Event> events = _context.Events.Include(e => e.Location)
                .OrderByDescending(e => e.EventStart);
            return events;
        }
    }
}
