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

        public async Task<Location> Create(CreateLocationViewModel model)
        {
            Location location = new Location()
            {
                Name = model.Name,
                Address = model.Address,
                Description = model.Description,
                X = double.Parse(model.X.Replace('.', ',')),
                Y = double.Parse(model.Y.Replace('.', ','))
            };
            var categories = model.LocationCategories.Where(x => x.Selected).Select(x => new { Id = x.Value, Name = x.Text });
            if (categories.Count() == 0)
            {
                var category = _db.LocationCategories.FirstOrDefault(c => c.Name == "Другое");
                var categoriesJson = JsonConvert.SerializeObject(new List<LocationCategory> { category });
                location.Categories = categoriesJson;
            }   
            else
            {
                var categoriesJson = JsonConvert.SerializeObject(categories);
                location.Categories = categoriesJson;
            }
            
            List<string> phones = new List<string>();
            phones = model.PhoneNums.Split(',').ToList();
            var phoneNumbersJson = JsonConvert.SerializeObject(phones);
            location.Phones = phoneNumbersJson;

            _db.Locations.Add(location);
            cache.Set(location.Id, location, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            await _db.SaveChangesAsync();
            return location;
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