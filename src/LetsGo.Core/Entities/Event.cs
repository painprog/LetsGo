using System;
using System.ComponentModel.DataAnnotations;
using LetsGo.Core.Entities.Enums;

namespace LetsGo.Core.Entities
{
    public class Event : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EventStart { get; set; }
        public DateTime EventEnd { get; set; }
        public string PosterImage { get; set; }
        public string Categories { get; set; }//json
        public int AgeLimit { get; set; }
        public int TicketLimit { get; set; }

        public virtual int StatusId
        {
            get => (int)this.Status;
            set => Status = (Status)value;
        }
        [EnumDataType(typeof(Status))]
        [Required]
        public Status Status { get; set; }
        public DateTime StatusUpdate { get; set; }
        public string StatusDescription { get; set; }

        public Location Location { get; set; }
        public int LocationId { get; set; }

        public int OrganizerId { get; set; }
        public User Organizer { get; set; } 
        public double MinPrice { get; set; }
        public int Sold { get; set; }
        public int Count { get; set; }
    }
}
