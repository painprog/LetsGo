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
                pathImage = "/posters/" + name;
                using (var fileStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\" + pathImage), FileMode.Create))
                    await eventView.File.CopyToAsync(fileStream);
            }
            else pathImage = "/images/gradient.jpeg";

            string categoriesJson = String.Empty;
            if (eventView.Categories.Where(x => x.Selected).Count() == 0)
            {
                var category = _goContext.EventCategories.FirstOrDefault(c => c.Name == "Другое");
                categoriesJson = JsonConvert.SerializeObject(new List<EventCategory> { category });
            }
            else
            {
                var categories = eventView.Categories.Where(x => x.Selected).Select(x => new { Id = x.Value, Name = x.Text });
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

        public async Task<List<EventTicketType>> AddEventTicketTypes(string eventId, List<EventTicketType> ticketTypes)
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

        public async Task DeleteEventTicketTypes(string[] IdsForDelete)
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

            var categories = _goContext.EventCategories.Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }).ToList();
            var other = categories.FirstOrDefault(l => l.Text == "Другое");
            categories.Remove(other);
            categories.Add(other);
            foreach (var item in System.Text.Json.JsonSerializer.Deserialize<List<EventCategory>>(@event.Categories))
            {
                categories.FirstOrDefault(c => c.Text == item.Name).Selected = true;
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
                EventCategories = categories,
                AgeLimit = @event.AgeLimit,
                TicketLimit = @event.TicketLimit,
                StatusId = @event.StatusId,
                Status = @event.Status,
                //CategoriesList = CategoriesList,
                Location = _goContext.Locations.FirstOrDefault(e => e.Id == @event.Location.Id).Name,  // change
                TicketsExist = EventTicketTypes(@event.Id).Result
        };
            return editEvent;
        }

        public async Task<Event> EditEvent(EditEventViewModel model)
        {
            Event @event = GetEvent(model.Id).Result;

            if (model.File != null)
            {
                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\" + @event.PosterImage));
                string filename = GenerateCode() + Path.GetExtension(model.File.FileName);
                filename = "/posters/" + filename;
                using (var fileStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\" + filename), FileMode.Create))
                    await model.File.CopyToAsync(fileStream);
                @event.PosterImage = filename;
            }

            var categories = model.EventCategories.Where(x => x.Selected).Select(x => new
            {
                Id = x.Value,
                Name = x.Text
            });
            if (categories.Count() == 0)
            {
                var category = _goContext.EventCategories.FirstOrDefault(c => c.Name == "Другое");
                var categoriesJson = JsonConvert.SerializeObject(new List<EventCategory> { category });
                @event.Categories = categoriesJson;
            }
            else
            {
                var categoriesJson = JsonConvert.SerializeObject(categories);
                @event.Categories = categoriesJson;
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

        public async Task<List<Event>> GetEvents(int userId)
        {
            List<Event> Events = new List<Event>();
            Events = await _goContext.Events.Include(e => e.Location).Where(p => p.OrganizerId == userId).ToListAsync();
            return Events;
        }

        //public async Task<List<EventCategory>> GetEventCategories(string jsonEventCategories)
        //{
        //    string eventCategories = System.Text.Json.JsonSerializer.Deserialize<string>(jsonEventCategories);
        //    List<string> CategoriesList = new List<string>();
        //    if (eventCategories.Contains(','))
        //    {
        //        string[] catesgInArray = eventCategories.Split(new char[] { ',' });
        //        CategoriesList.AddRange(catesgInArray);
        //    }
        //    else
        //        CategoriesList.Add(eventCategories);
        //    List<EventCategory> Categories = new List<EventCategory>();
        //    foreach (var item in CategoriesList)
        //        Categories.Add(await _goContext.EventCategories.FirstOrDefaultAsync(e => e.Name == item));
        //    return Categories;
        //}

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

        public async Task<Event> GetEvent(string id)
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

        public async Task<EventTicketType> GetEventTicketType(string id)
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

        public async Task<List<EventTicketType>> EventTicketTypes(string eventId)
        {
            return _goContext.EventTicketTypes.Where(e => e.EventId == eventId).ToList();
        }

        public async Task<Location> GetLocation(string id)
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
    }
}