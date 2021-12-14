using LetsGo.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using LetsGo.Core.Entities;
using LetsGo.DAL;

namespace LetsGo.Services
{
    public class LocationsService
    {
        private readonly ApplicationDbContext _db;
        public LocationsService(ApplicationDbContext db)
        {
            _db = db;
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
    }
}