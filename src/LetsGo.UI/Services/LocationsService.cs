using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetsGo.Core;
using LetsGo.Core.Entities;
using LetsGo.DAL;
using LetsGo.UI.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace LetsGo.UI.Services
{
    public class LocationsService
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private IMemoryCache cache;

        public LocationsService(ApplicationDbContext db, IMemoryCache cache, IUnitOfWorkFactory unitOfWorkFactory)
        {
            _db = db;
            this.cache = cache;
            _unitOfWorkFactory = unitOfWorkFactory;
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

            using (var uow = _unitOfWorkFactory.MakeUnitOfWork())
            {
                if (categories.Count() == 0)
                {
                    var category = uow.LocationCategories.Find(c => c.Name == "Другое").FirstOrDefault();
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

                uow.Locations.Add(location);
            cache.Set(location.Id, location, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            await _db.SaveChangesAsync();
            return location;
        }
        }

        public async Task<Location> GetLocation(int id)
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

        public async Task<List<Event>> GetLocationEvents(int locationId)
        {
            List<Event> Events = new List<Event>();
            Events = await _db.Events.Include(e => e.Location).Where(p => p.LocationId == locationId).ToListAsync();
            return Events;
        }

    }
}