using System.Collections.Generic;
using LetsGo.Core.Entities;

namespace LetsGo.UI.ViewModels
{
    public class IndexPageViewModel
    {
        public List<Event> Concerts { get; set; }
        public List<Event> Festivals { get; set; }
        public List<Event> Performances { get; set; }
        public List<Event> ForChildren { get; set; }
        public List<Location> Locations { get; set; }
        public Dictionary<int?, List<EventCategory>> CategoriesDictionary { get; set; }

        public string SelectedDates { get; set; }
        public string SelectedCategories { get; set; }
    }
}
