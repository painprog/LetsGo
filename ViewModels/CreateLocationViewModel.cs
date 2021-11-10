using LetsGo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace LetsGo.ViewModels
{
    public class CreateLocationViewModel
    {
        public Location Location { get; set; }
        public IList<SelectListItem> LocationCategories { get; set; }
        public string PhoneNums { get; set; }
    }
}
