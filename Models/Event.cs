using LetsGo.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LetsGo.Models
{
    public class Event
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
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

        public Location Location { get; set; }
        public string LocationId { get; set; }

        public string OrganizerId { get; set; }
        public User Organizer { get; set; } 
    }
}
