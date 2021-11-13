﻿using LetsGo.Models;
using LetsGo.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace LetsGo.Services
{
    public class LocationsService
    {
        private readonly LetsGoContext _db;
        public LocationsService(LetsGoContext db)
        {
            _db = db;
        }

        public async void Create(CreateLocationViewModel model)
        {
            var categories = model.LocationCategories.Where(x => x.Selected).Select(x => new
            {
                Id = x.Value,
                Name = x.Text
            }).ToList();
            var categoriesJson = JsonConvert.SerializeObject(categories);
            model.Location.Categories = categoriesJson;

            List<string> phones = new List<string>();
            if (model.PhoneNums != null)
                phones = model.PhoneNums.Split(',').ToList();
            if (model.Location.Phones != null)
                phones.Add(model.Location.Phones);
            var phoneNumbersJson = JsonConvert.SerializeObject(phones);
            model.Location.Phones = phoneNumbersJson;

            _db.Locations.Add(model.Location);
            _db.SaveChanges();
        }
    }
}