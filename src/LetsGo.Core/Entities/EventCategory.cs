namespace LetsGo.Core.Entities
{
    public class EventCategory : Entity
    {
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public bool HasParent { get; set; }
    }
}
