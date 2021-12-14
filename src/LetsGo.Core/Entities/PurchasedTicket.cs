using System;

namespace LetsGo.Core.Entities
{
    public class PurchasedTicket : Entity
    {
        public string TicketIdentifier { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerName { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string QR { get; set; }
        public bool Scanned { get; set; }

        public EventTicketType EventTicketType { get; set; }
        public int EventTicketTypeId { get; set; }
    }
}
