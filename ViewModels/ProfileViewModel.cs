using LetsGo.Enums;
using LetsGo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.ViewModels
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public List<Event> Events { get; set; }
        public DateTime DateTimeBefore { get; set; }
        public DateTime DateTimeFrom { get; set; }
        public Status Status { get; set; }
        public List<EventCategory> EventCategories { get; set; }
        public string EventCategory { get; set; }
    }
}
