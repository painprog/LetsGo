using System;
using System.Collections.Generic;
using LetsGo.Core.Entities;
using LetsGo.Core.Entities.Enums;

namespace LetsGo.UI.ViewModels
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public bool IsOrganizer { get; set; }
        public List<Event> Events { get; set; }
        public DateTime DateTimeBefore { get; set; }
        public DateTime DateTimeFrom { get; set; }
        public Status Status { get; set; }
        public List<EventCategory> EventCategories { get; set; }
        public string EventCategory { get; set; }
        public Dictionary<string, Status> Stats { get; set; }
        public string EventCategs { get; set; }
    }
}
