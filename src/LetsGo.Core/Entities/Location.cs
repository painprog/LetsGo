namespace LetsGo.Core.Entities
{
    public class Location : Entity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string Phones { get; set; }  // json
        public string Description { get; set; }
        public string Categories { get; set; }  // json
        public string LocationImage { get; set; }
    }
}
