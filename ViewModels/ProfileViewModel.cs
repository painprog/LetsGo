using LetsGo.Models;
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
    }
}
