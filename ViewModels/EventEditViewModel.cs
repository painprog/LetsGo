using LetsGo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.ViewModels
{
    public class EventEditViewModel
    {
        public Event Event { get; set; }
        public List<EventTicketType> EventTicketTypes { get; set; }
        public string Tickets { get; set; }
        public string DeletedTickets { get; set; }
    }
}
