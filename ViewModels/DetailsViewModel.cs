using LetsGo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.ViewModels
{
    public class DetailsViewModel
    {
        public Event Event { get; set; }
        public List<EventTicketType> EventTickets { get; set; }
        public List<LocationCategory> LocationCategories { get; set; }
        public List<EventCategory> EventCategories { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
