using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.Models
{
    public class EventTicketType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
        public int Sold { get; set; }
        public double Price { get; set; }

        public string EventId { get; set; }
        public Event Event { get; set; }
    }
}
