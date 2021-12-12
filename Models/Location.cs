using System.ComponentModel.DataAnnotations.Schema;

namespace LetsGo.Models
{
    public class Location
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
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
