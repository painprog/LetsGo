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
    }
}
