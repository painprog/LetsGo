using System.Collections.Generic;
using LetsGo.Core.Entities;

namespace LetsGo.ViewModels
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public List<Event> Events { get; set; }
    }
}
