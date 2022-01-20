using LetsGo.Core.Entities;
using System.Collections.Generic;

namespace LetsGo.UI.ViewModels
{
    public class AfishaViewModel
    {
        public List<Event> Events { get; set; }
        public Dictionary<int?, List<EventCategory>> CategoriesDictionary { get; set; }
    }
}
