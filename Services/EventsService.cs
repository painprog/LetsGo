using LetsGo.Models;
using LetsGo.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private IWebHostEnvironment _appEnvironment;

        public EventsService(LetsGoContext goContext, IWebHostEnvironment appEnvironment)
        {
            _goContext = goContext;
            _appEnvironment = appEnvironment;
        }

        public async Task<Event> AddEvent(AddEventViewModel eventView)
        {
            string UCimg = GenerateCode();
            string name = UCimg + '.'+ eventView.File.ContentType.Substring(eventView.File.ContentType.IndexOf('/') + 1);
            string pathImage = "/posters/" + name;
            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + pathImage, FileMode.Create))
                await eventView.File.CopyToAsync(fileStream);

            string jsonCateg = string.Empty;
            if (eventView.Categories.Length > 0)
                jsonCateg = JsonSerializer.Serialize(eventView.Categories);

            Event @event = new Event
            {
                Name = eventView.Name,
                Description = eventView.Description,
                CreatedAt = DateTime.Now,
                EventStart = eventView.EventStart,
                EventEnd = eventView.EventEnd,
                PosterImage = pathImage,
                Categories = jsonCateg,
                AgeLimit = Convert.ToInt32(eventView.AgeLimit),
                TicketLimit = eventView.TicketLimit,
                LocationId = eventView.LocationId
            };
            await _goContext.Events.AddAsync(@event);
            await _goContext.SaveChangesAsync();
            
            return @event;
        }

        public async Task<List<EventTicketType>> AddEventTicketTypes(string eventId, List<EventTicketType> ticketTypes)
        {
            foreach (var item in ticketTypes)
            {
                item.EventId = eventId;
                item.Sold = 0;
                _goContext.EventTicketTypes.Add(item);
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
    }
}
