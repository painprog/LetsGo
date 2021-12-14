namespace LetsGo.Core.Entities
{
    public class EventTicketType : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
        public int Sold { get; set; }
        public double Price { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}
