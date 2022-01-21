using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetsGo.Core.Entities;
using LetsGo.Core.Entities.Enums;
using LetsGo.DAL;
using LetsGo.UI.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace LetsGo.UI.Services
{
    public class EventsService
    {
        private readonly ApplicationDbContext _goContext;
        private IMemoryCache cache;

        public EventsService(ApplicationDbContext goContext, IMemoryCache cache)
        {
            _goContext = goContext;
            this.cache = cache;
        }

        public async Task<Event> AddEvent(AddEventViewModel eventView)
        {
            string pathImage = String.Empty;
            if (eventView.File != null)
            {
                string name = GenerateCode() + Path.GetExtension(eventView.File.FileName);
                pathImage = "/events/" + name;
                using (var fileStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\" + pathImage), FileMode.Create))
                    await eventView.File.CopyToAsync(fileStream);
            }
            else pathImage = "/images/gradient.jpeg";

            string categoriesJson = String.Empty;
            
            if (eventView.SelectedCategoryIds == null)
            {
                var category = _goContext.EventCategories.FirstOrDefault(c => c.Name == "Другое");
                categoriesJson = JsonConvert.SerializeObject(new List<EventCategory> { category });
            }
            else
            {
                string[] categoryIds = eventView.SelectedCategoryIds.Split(',');
                List<EventCategory> categories = new List<EventCategory>();
                foreach(string id in categoryIds)
                {
                    categories.Add(await _goContext.EventCategories
                        .FirstOrDefaultAsync(c => c.Id == Convert.ToInt32(id)));                    
                }
                categoriesJson = JsonConvert.SerializeObject(categories);
            }

            Event @event = new Event
            {
                Name = eventView.Name,
                Description = eventView.Description,
                CreatedAt = DateTime.Now,
                EventStart = (DateTime)eventView.EventStart,
                EventEnd = (DateTime)eventView.EventEnd,
                PosterImage = pathImage,
                Categories = categoriesJson,
                AgeLimit = Convert.ToInt32(eventView.AgeLimit),
                TicketLimit = eventView.TicketLimit,
                Status = Status.New,
                StatusUpdate = DateTime.Now,
                LocationId = _goContext.Locations.FirstOrDefault(l => l.Name == eventView.Location).Id,  // change
                OrganizerId = eventView.OrganizerId
            };

            if(eventView.Tickets != "[]")
            {
                var tickets = JsonConvert.DeserializeObject<List<EventTicketType>>(eventView.Tickets);
                @event.Count = eventView.TicketLimit;
                @event.MinPrice = tickets.OrderBy(t => t.Price).First().Price;
            }

            await _goContext.Events.AddAsync(@event);
            await _goContext.SaveChangesAsync();
            cache.Set(@event.Id, @event, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

            return @event;
        }

        public async Task<List<EventTicketType>> AddEventTicketTypes(int eventId, List<EventTicketType> ticketTypes)
        {
            foreach (var item in ticketTypes)
            {
                item.EventId = eventId;
                _goContext.EventTicketTypes.Add(item);
                cache.Set(item.Id, item, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }

            await _goContext.SaveChangesAsync();

            return ticketTypes;
        }

        public async Task<List<EventTicketType>> UpdateEventTicketTypes(int eventId, List<EventTicketType> ticketTypes)
        {
            int count = 0;
            foreach (var type in ticketTypes)
            {
                if (type.Id == default)
                {
                    type.EventId = eventId;
                    _goContext.EventTicketTypes.Add(type);
                }
                else
                {
                    _goContext.EventTicketTypes.Update(type);
                }
                count += type.Count;
                cache.Set(type.Id, type, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }

            Event @event = GetEvent(eventId).Result;
            @event.Count = count;
            _goContext.Events.Update(@event);
            await _goContext.SaveChangesAsync();

            return ticketTypes;
        }

        public async Task DeleteEventTicketTypes(int[] IdsForDelete)
        {
            foreach (var id in IdsForDelete)
            {
                EventTicketType ticketType = GetEventTicketType(id).Result;
                _goContext.EventTicketTypes.Remove(ticketType);
            }
            await _goContext.SaveChangesAsync();
        }

        public async Task<EditEventViewModel> MakeEditEventViewModel(int id)
        {
            Event @event = GetEvent(id).Result;

            var parentCategories = _goContext.EventCategories
                .Where(c => c.HasParent == false)
                .OrderBy(c => c.Id).ToList();

            var childCategories = _goContext.EventCategories
                .Where(c => c.HasParent == true)
                .OrderBy(c => c.ParentId).ToList();

            var selectedCategories = JsonConvert.DeserializeObject<List<EventCategory>>(@event.Categories);
            List<string> scIds = new List<string>();
            foreach(var category in selectedCategories)
            {
                scIds.Add(category.Id.ToString());
            }

            EditEventViewModel editEvent = new EditEventViewModel
            {
                Id = @event.Id,
                Name = @event.Name,
                Description = @event.Description,
                CreatedAt = @event.CreatedAt,
                EventStart = @event.EventStart,
                EventEnd = @event.EventEnd,
                PosterImage = @event.PosterImage,
                ParentCategories = parentCategories,
                ChildCategories = childCategories,
                SelectedCategoryIds = string.Join(',', scIds),
                AgeLimit = @event.AgeLimit,
                TicketLimit = @event.TicketLimit,
                StatusId = @event.StatusId,
                Status = @event.Status,
                Location = _goContext.Locations.FirstOrDefault(e => e.Id == @event.Location.Id).Name,  // change
                TicketsExist = EventTicketTypes(@event.Id)
        };
            return editEvent;
        }

        public async Task<Event> EditEvent(EditEventViewModel model)
        {
            Event @event = GetEvent(model.Id).Result;

            if (model.File != null)
            {
                File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\" + @event.PosterImage));
                string filename = GenerateCode() + Path.GetExtension(model.File.FileName);
                filename = "/events/" + filename;
                using (var fileStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\" + filename), FileMode.Create))
                    await model.File.CopyToAsync(fileStream);
                @event.PosterImage = filename;
            }            

            if (model.SelectedCategoryIds == null)
            {
                var category = _goContext.EventCategories.FirstOrDefault(c => c.Name == "Другое");
                @event.Categories = JsonConvert.SerializeObject(new List<EventCategory> { category });
            }
            else
            {
                string[] categoryIds = model.SelectedCategoryIds.Split(',');
                List<EventCategory> categories = new List<EventCategory>();
                foreach (string id in categoryIds)
                {
                    categories.Add(await _goContext.EventCategories
                        .FirstOrDefaultAsync(c => c.Id == Convert.ToInt32(id)));
                }
                @event.Categories = JsonConvert.SerializeObject(categories);
            }

            @event.Name = model.Name;
            @event.Description = model.Description;
            @event.CreatedAt = DateTime.Now;
            @event.EventStart = model.EventStart;
            @event.EventEnd = model.EventEnd;
            @event.AgeLimit = model.AgeLimit;
            @event.TicketLimit = model.TicketLimit;
            @event.Status = Status.New;
            @event.LocationId = _goContext.Locations.FirstOrDefault(l => l.Name == model.Location).Id;  // change

            _goContext.Events.Update(@event);
            await _goContext.SaveChangesAsync();
            cache.Set(@event.Id, @event, new MemoryCacheEntryOptions());
            return @event;
        }

        public async Task<bool> ChangeStatus(string status, int eventId, string cause)
        {
            var @event = GetEvent(eventId).Result;
            if (status != null && @event != null)
            {
                switch (status)
                {
                    case "Published":
                        @event.Status = Status.Published;
                        @event.StatusDescription = null;
                        break;
                    case "Rejected":
                        @event.Status = Status.Rejected;
                        @event.StatusDescription = cause;
                        break;
                    case "UnPublished":
                        @event.Status = Status.UnPublished;
                        @event.StatusDescription = cause;
                        break;
                    case "Edited":
                        @event.Status = Status.Edited;
                        @event.StatusDescription = cause;
                        break;
                    case "ReviewUnPublished":
                        @event.Status = Status.ReviewUnPublished;
                        @event.StatusDescription = cause;
                        break;
                    case "ReviewPublished":
                        @event.Status = Status.ReviewPublished;
                        @event.StatusDescription = cause;
                        break;
                    default:
                        break;
                }
                @event.StatusUpdate = DateTime.Now;

                _goContext.Events.Update(@event);
                await _goContext.SaveChangesAsync();

                return true;
            }
            return false;
        }

        
        // default methods

        public async Task<List<Event>> Events()
        {
            var events = _goContext.Events.Include(e => e.Location).ToList();
            foreach (var item in events)
            {
                if (item.EventEnd < DateTime.Now) item.Status = Status.Expired;
                _goContext.Events.Update(item);
            }
            await _goContext.SaveChangesAsync();
            return events;
        }

        public async Task<List<Event>> GetEvents(int userId)
        {
            List<Event> Events = new List<Event>();
            Events = await _goContext.Events.Include(e => e.Location).Where(p => p.OrganizerId == userId).ToListAsync();
            return Events;
        }

        public async Task<Event> GetEvent(int id)
        {
            Event Event = null;
            if (!cache.TryGetValue(id, out Event))
            {
                Event = await _goContext.Events.Include(e => e.Location).FirstOrDefaultAsync(p => p.Id == id);
                if (Event != null)
                {
                    cache.Set(Event.Id, Event,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return Event;
        }

        public async Task<EventTicketType> GetEventTicketType(int id)
        {
            EventTicketType Event = null;
            if (!cache.TryGetValue(id, out Event))
            {
                Event = await _goContext.EventTicketTypes.FirstOrDefaultAsync(p => p.Id == id);
                if (Event != null)
                {
                    cache.Set(Event.Id, Event,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return Event;
        }

        public List<EventTicketType> EventTicketTypes(int eventId)
        {
            return _goContext.EventTicketTypes.Where(e => e.EventId == eventId).ToList();
        }

        public async Task<Location> GetLocation(int id)
        {
            Location location = null;
            if (!cache.TryGetValue(id, out location))
            {
                location = await _goContext.Locations.FirstOrDefaultAsync(l => l.Id == id);
                if (location != null)
                {
                    cache.Set(location.Id, location,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return location;
        }

        public async Task<EventCategory> GetEventCategory(int id)
        {
            EventCategory category = null;
            if (!cache.TryGetValue(id, out category))
            {
                category = await _goContext.EventCategories.FirstOrDefaultAsync(c => c.Id == id);
                if (category != null)
                {
                    cache.Set(category.Id, category,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return category;
        }

        // additional methods

        public static string GenerateCode()
        {
            StringBuilder builder = new StringBuilder(6);
            Random random = new Random();
            for (var i = 1; i <= 12; i++)
            {
                builder.Append(random.Next(10));
            }
            string UC = builder.ToString();
            return UC;
        }

        public IQueryable<Event> QueryableEventsAfterFilter(
            List<int> EventCategories, Status Status, DateTime DateTimeFrom, DateTime DateTimeBefore
        )
        {
            IQueryable<Event> Events = _goContext.Events.Include(e => e.Location).OrderByDescending(e => e.EventStart);

            if (EventCategories.Count > 0)
            {
                List<Event> FilteredEvents = new List<Event>();

                List<EventCategory> selectedCategories = new List<EventCategory>();
                foreach (var id in EventCategories)
                    selectedCategories.Add(GetEventCategory(id).Result);

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
            else if (DateTimeBefore != DateTime.MinValue)
                Events = Events.Where(e => e.EventStart <= DateTimeBefore);

            return Events;
        }
    }
}