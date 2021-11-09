using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.Models
{
    public class PurchasedTicket
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerName { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string QR { get; set; }
        public bool Scanned { get; set; }

        public EventTicketType EventTicketType { get; set; }
        public string EventTicketTypeId { get; set; }
    }
}
