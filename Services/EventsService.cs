using LetsGo.Enums;
using LetsGo.Models;
using LetsGo.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LetsGo.Services
{
    public class EventsService
    {
        private readonly LetsGoContext _goContext;
        private IMemoryCache cache;

        public EventsService(LetsGoContext goContext, IMemoryCache cache)
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
            else
                pathImage = "/images/gradient.jpeg";

            string categoriesJson = String.Empty;
            if (eventView.Categories.Where(x => x.Selected == true).Count() == 0)
            {
                var categories = _goContext.EventCategories.Where(c => c.Name == "Другое");
                categoriesJson = JsonConvert.SerializeObject(categories);
            }
            else
            {
                var categories = eventView.Categories.Where(x => x.Selected).Select(x => new
                {
                    Id = x.Value,
                    Name = x.Text
                });
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
                LocationId = _goContext.Locations.FirstOrDefault(l => l.Name == eventView.Location).Id,
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
            cache.Set(@event.Id, @event, new MemoryCacheEntryOptions());

            return @event;
        }


        public async Task<List<EventTicketType>> AddEventTicketTypes(string eventId, List<EventTicketType> ticketTypes)
        {
            foreach (var item in ticketTypes)
            {
                item.EventId = eventId;
                item.Sold = 0;
                _goContext.EventTicketTypes.Add(item);
                cache.Set(item.Id, item, new MemoryCacheEntryOptions());
            }

            await _goContext.SaveChangesAsync();

            return ticketTypes;
        }

        public async Task<List<EventTicketType>> UpdateEventTicketTypes(string eventId, List<EventTicketType> ticketTypes)
        {
            foreach (var type in ticketTypes)
            {
                if (String.IsNullOrEmpty(type.Id))
                {
                    type.Id = null;
                    type.EventId = eventId;
                    _goContext.EventTicketTypes.Add(type);
                }
                else
                {
                    _goContext.EventTicketTypes.Update(type);
                }
                cache.Set(type.Id, type, new MemoryCacheEntryOptions());
            }
            await _goContext.SaveChangesAsync();
            return ticketTypes;
        }

        public async Task DeleteEventTicketTypes(string[] deletedIds)
        {
            foreach (var id in deletedIds)
            {
                EventTicketType ticketType = await _goContext.EventTicketTypes.FirstOrDefaultAsync(e => e.Id == id);
                _goContext.EventTicketTypes.Remove(ticketType);
            }
            await _goContext.SaveChangesAsync();
        }


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

        public async Task<EditEventViewModel> MakeEditEventViewModel(string id)
        {
            Event @event = await _goContext.Events.FirstOrDefaultAsync(e => e.Id == id);

            //string categs = System.Text.Json.JsonSerializer.Deserialize<string>(@event.Categories);
            //List<string> CategoriesList = new List<string>();
            //if (categs.Contains(','))
            //{ 
            //    string[] catesgInArray = categs.Split(new char[] { ',' });
            //    CategoriesList.AddRange(catesgInArray);
            //}
            //else
            //    CategoriesList.Add(categs);
           
            EditEventViewModel editEvent = new EditEventViewModel
            {
                Id = @event.Id,
                Name = @event.Name,
                Description = @event.Description,
                CreatedAt = @event.CreatedAt,
                EventStart = @event.EventStart,
                EventEnd = @event.EventEnd,
                PosterImage = @event.PosterImage,
                Categories = @event.Categories,
                AgeLimit = @event.AgeLimit,
                TicketLimit = @event.TicketLimit,
                StatusId = @event.StatusId,
                Status = @event.Status,
                //CategoriesList = CategoriesList,
                Location = _goContext.Locations.FirstOrDefault(e => e.Id == @event.Location.Id).Name,
                TicketsExist = _goContext.EventTicketTypes.Where(e => e.EventId == @event.Id).ToList()
        };
            return editEvent;
        }

        public async Task<Event> EditEvent(EditEventViewModel eventView)
        {
            Event @event = await _goContext.Events.FirstOrDefaultAsync(e => e.Id == eventView.Id);

            if (eventView.File != null)
            {
                string name = GenerateCode() + '.' + Path.GetExtension(eventView.File.FileName);
                eventView.PosterImage = "/posters/" + name;
                using (var fileStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\" + eventView.PosterImage), FileMode.Create))
                    await eventView.File.CopyToAsync(fileStream);
                @event.PosterImage = eventView.PosterImage;
            }

            string jsonCateg = string.Empty;
            jsonCateg = eventView.Categories != null ? System.Text.Json.JsonSerializer.Serialize(eventView.Categories) : "";

            @event.Name = eventView.Name;
            @event.Description = eventView.Description;
            @event.CreatedAt = DateTime.Now;
            @event.EventStart = eventView.EventStart;
            @event.EventEnd = eventView.EventEnd;              
            @event.Categories = jsonCateg;
            @event.AgeLimit = eventView.AgeLimit;
            @event.TicketLimit = eventView.TicketLimit;
            @event.Status = Status.Edited;
            @event.LocationId = _goContext.Locations.FirstOrDefault(l => l.Name == eventView.Location).Id;

            _goContext.Events.Update(@event);
            await _goContext.SaveChangesAsync();
            cache.Set(@event.Id, @event, new MemoryCacheEntryOptions());
            return @event;
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

        public async Task<List<Event>> GetEvents(string userId)
        {
            List<Event> Events = new List<Event>();
            Events = await _goContext.Events.Include(e => e.Location).Where(p => p.OrganizerId == userId).ToListAsync();
            return Events;
        }

        public async Task<List<EventCategory>> GetEventCategories(string jsonEventCategories)
        {
            string eventCategories = System.Text.Json.JsonSerializer.Deserialize<string>(jsonEventCategories);
            List<string> CategoriesList = new List<string>();
            if (eventCategories.Contains(','))
            {
                string[] catesgInArray = eventCategories.Split(new char[] { ',' });
                CategoriesList.AddRange(catesgInArray);
            }
            else
                CategoriesList.Add(eventCategories);
            List<EventCategory> Categories = new List<EventCategory>();
            foreach (var item in CategoriesList)
                Categories.Add(await _goContext.EventCategories.FirstOrDefaultAsync(e => e.Name == item));
            return Categories;
        }

        public async Task<bool> ChangeStatus(string status, string eventId, string cause)
        {
            var @event = await _goContext.Events.FirstOrDefaultAsync(e => e.Id == eventId);
            if (status != null && @event != null)
            {
                switch (status)
                {
                    case "Publish":
                        @event.Status = Status.Published;
                        @event.StatusDescription = "Ok";
                        break;
                    case "Rejected":
                        @event.Status = Status.Rejected;
                        @event.StatusDescription = cause;
                        break;
                    case "Unpublish":
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
    }
}