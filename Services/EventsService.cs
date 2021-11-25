﻿using LetsGo.Enums;
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
    }
}
