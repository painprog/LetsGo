using LetsGo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.UI.ViewModels
{
    public class FilterViewModel
    {
        public Dictionary<int?, List<EventCategory>> CategoriesDictionary { get; set; }

        public string SelectedDates { get; set; }
        public string SelectedCategories { get; set; }
    }
}
