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

        public IQueryable<Event> QueryableEventsAfterFilter(List<int> EventCategories, Status Status,
           DateTime DateTimeFrom, DateTime DateTimeBefore)
        {
            IQueryable<Event> Events = _context.Events.Include(e => e.Location).OrderByDescending(e => e.EventStart);

            if (EventCategories.Count > 0)
            {
                List<EventCategory> eventCategories = new List<EventCategory>();
                foreach (var item in EventCategories)
                    eventCategories.Add(_context.EventCategories.FirstOrDefault(e => e.Id == item));

                List<EventCategory> mainEventCategories = eventCategories.Where(e => !e.HasParent).ToList();
                List<EventCategory> subEventCategories = eventCategories.Where(e => e.HasParent).ToList();

                var categoriesGroups = subEventCategories.GroupBy(e => e.ParentId);
                List<Event> EventsAfterFiltr = new List<Event>();

                if (mainEventCategories.Count > 0)
                {
                    int j = 0;
                    foreach (var item in mainEventCategories)
                    {
                        Events = Events.Where(e => e.Categories.Contains(item.Name));
                        EventsAfterFiltr.AddRange(Events);
                        Events = GetEvents();
                        j++;
                    }
                    if (j == mainEventCategories.ToList().Count && subEventCategories.Count == 0)
                        Events = EventsAfterFiltr.AsQueryable();
                }
                if (subEventCategories.Count > 0)
                {
                    int count = 0;
                    EventCategory eventCategory = new EventCategory();
                    foreach (var g in categoriesGroups)
                    {
                        count++;
                        if (mainEventCategories.FirstOrDefault(e => e.Id == g.Key) != null)
                        {
                            eventCategory = _context.EventCategories.FirstOrDefault(e => e.Id == g.Key);
                            for (int i = 0; i < EventsAfterFiltr.Count; i++)
                            {
                                if (EventsAfterFiltr[i].Categories.Contains(eventCategory.Name))
                                    EventsAfterFiltr.Remove(EventsAfterFiltr[i]);
                            }
                        }
                        for (int i = 0; i < g.ToList().Count; i++)
                        {
                            Events = Events.Where(x => x.Categories.Contains(g.ToList()[i].Name));
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
