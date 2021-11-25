using LetsGo.Models;
using System.Collections.Generic;

namespace LetsGo.ViewModels
{
    public class IndexPageViewModel
    {
        public List<Event> Concerts { get; set; }
        public List<Event> Festivals { get; set; }
        public List<Event> Performances { get; set; }
        public List<Event> ForChildren { get; set; }

        public List<Location> Locations { get; set; }
    }
}
