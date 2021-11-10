using LetsGo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.ViewModels
{
    public class DetailsViewModel
    {
        public Event Event { get; set; }
        public EventTicketType eventTickets { get; set; }
        public List<LocationCategory> LocationCategories { get; set; }
        public List<EventCategory> EventCategories { get; set; }
    }
}
