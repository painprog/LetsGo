using LetsGo.Models;
using LetsGo.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.Services
{
    public class LocationsService
    {
        private readonly LetsGoContext _db;
        private IMemoryCache cache;
        public LocationsService(LetsGoContext db, IMemoryCache cache)
        {
            _db = db;
            this.cache = cache;
        }

        public async void Create(CreateLocationViewModel model)
        {
            Location location = new Location()
            {
                Name = model.Name,
                Address = model.Address,
                Description = model.Description
            };
            if (model.LocationCategories.Where(x => x.Selected == true).Count() == 0)
            {
                var categories = _db.LocationCategories.Where(c => c.Name == "Другое");
                var categoriesJson = JsonConvert.SerializeObject(categories);
                location.Categories = categoriesJson;
            }   
            else
            {
                var categories = model.LocationCategories.Where(x => x.Selected).Select(x => new
                {
                    Id = x.Value,
                    Name = x.Text
                });
                var categoriesJson = JsonConvert.SerializeObject(categories);
                location.Categories = categoriesJson;
            }
            
            List<string> phones = new List<string>();
            if (model.PhoneNums != null)
                phones = model.PhoneNums.Split(',').ToList();
            if (model.Phones != null)
                phones.Add(model.Phones);
            var phoneNumbersJson = JsonConvert.SerializeObject(phones);
            location.Phones = phoneNumbersJson;

            _db.Locations.Add(location);
            _db.SaveChanges();
        }
        public async Task<Location> GetLocation(string id)
        {
            Location location = null;
            if (!cache.TryGetValue(id, out location))
            {
                location = await _db.Locations.FirstOrDefaultAsync(p => p.Id == id);
                if (location != null)
                {
                    cache.Set(location.Id, location,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return location;
        }
        public async Task<List<Event>> GetLocationEvents(string locationId)
        {
            List<Event> Events = new List<Event>();
            Events = await _db.Events.Include(e => e.Location).Where(p => p.LocationId == locationId).ToListAsync();
            return Events;
        }

    }
}