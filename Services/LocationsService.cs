using LetsGo.Models;
using LetsGo.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.Services
{
    public class LocationsService
    {
        private readonly LetsGoContext _db;
        public LocationsService(LetsGoContext db)
        {
            _db = db;
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
            var categories = model.LocationCategories.Where(x => x.Selected).Select(x => new
            {
                Id = x.Value,
                Name = x.Text
            });
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
            await _db.SaveChangesAsync();
            return location;
        }
    }
}