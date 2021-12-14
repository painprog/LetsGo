using LetsGo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetsGo.Core.Entities;

namespace LetsGo.ViewModels
{
    public class LocationDetailsViewModel
    {
        public Location Location { get; set; }
        //public DateTime MaxDate { get; set; }
        public List<Event> FutureEvents { get; set; }
        public List<Event> PastEvents { get; set; }
        public List<LocationCategory> LocationCategories { get; set; }
    }
}
