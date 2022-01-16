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

        public IQueryable<Event> QueryableEventsAfterFilter(
            List<int> EventCategories, Status Status, DateTime DateTimeFrom, DateTime DateTimeBefore
        )
        {
            IQueryable<Event> Events = _context.Events.Include(e => e.Location).OrderByDescending(e => e.EventStart);

            if (EventCategories.Count > 0)
            {
                List<Event> FilteredEvents = new List<Event>();

                List<EventCategory> selectedCategories = new List<EventCategory>();
                foreach (var id in EventCategories)
                    selectedCategories.Add(_service.GetEventCategory(id).Result);

                var categoriesDictionary = selectedCategories.GroupBy(c => c.ParentId).ToDictionary(g => g.Key.HasValue ? g.Key : -1, g => g.ToList());

                foreach (var item in categoriesDictionary)
                {
                    foreach (var category in item.Value)
                    {
                        if (!categoriesDictionary.ContainsKey(category.Id))
                        {
                            FilteredEvents.AddRange(Events.Where(e => e.Categories.Contains(category.Name)));
                        }
                    }
                }

                Events = FilteredEvents.AsQueryable();
            }
            if (Status != Status.NotDefined)
                Events = Events.Where(e => e.Status == Status);

            if (DateTimeFrom != DateTime.MinValue && DateTimeBefore != DateTime.MinValue)
                Events = Events.Where(e => e.EventStart >= DateTimeFrom && e.EventStart <= DateTimeBefore);
            else if (DateTimeFrom != DateTime.MinValue)
                Events = Events.Where(e => e.EventStart >= DateTimeFrom);
            else
                Events = Events.Where(e => e.EventStart <= DateTimeBefore);

            return Events;
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
