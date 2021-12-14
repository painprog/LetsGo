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

        public IQueryable<Event> QueryableEventsAfterFilter(List<string> EventCategories, Status Status,
           DateTime DateTimeFrom, DateTime DateTimeBefore)
        {
            IQueryable<Event> Events = _context.Events.Include(e => e.Location).OrderBy(e => e.Status).ThenByDescending(e => e.CreatedAt);

            if (EventCategories.Count > 0)
            {
                List<EventCategory> eventCategories = new List<EventCategory>();
                foreach (var item in EventCategories)
                    eventCategories.Add(_context.EventCategories.FirstOrDefault(e => e.Id == item));

                var categoriesGroups = eventCategories.GroupBy(e => e.ParentId);
                List<Event> EventsAfterFiltr = new List<Event>();

                int count = 0;
                foreach (var g in categoriesGroups)
                {
                    count++;
                    for (int i = 0; i < g.ToList().Count; i++)
                    {
                        Events = Events.Where(x => x.Categories.Contains(g.ToList()[i].Id));
                        if (Events.ToList().Count == 0)
                        {
                            Events = GetEvents();
                            break;
                        }
                        if (i == g.ToList().Count - 1)
                        {
                            EventsAfterFiltr.AddRange(Events);
                            Events = GetEvents();
                        }
                    }
                    if (count == categoriesGroups.ToList().Count)
                        Events = EventsAfterFiltr.AsQueryable();
                }
            }
            if (Status != Status.NotDefined)
                Events = Events.Where(e => e.Status == Status);
            if (DateTimeFrom != DateTime.MinValue)
                Events = Events.Where(e => e.EventStart >= DateTimeFrom);
            if (DateTimeBefore != DateTime.MinValue)
                Events = Events.Where(e => e.EventStart <= DateTimeBefore);
            if (DateTimeFrom != DateTime.MinValue && DateTimeBefore != DateTime.MinValue)
                Events = Events.Where(e => e.EventStart >= DateTimeFrom && e.EventStart <= DateTimeBefore);

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
                ["Обзор опубликован"] = Status.ReviewPublished,
                ["Обзор не опубликован"] = Status.ReviewUnPublished,
            };
            return Stats;
        }

        public IQueryable<Event> GetEvents()
        {
            IQueryable<Event> events = _context.Events.Include(e => e.Location)
                .OrderBy(e => e.Status)
                .ThenByDescending(e => e.CreatedAt);
            return events;
        }
    }
}
