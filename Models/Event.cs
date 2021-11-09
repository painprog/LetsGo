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

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Название")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Описание")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Время начала")]
        [JsonPropertyName("eventStart")]
        public DateTime EventStart { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Время конца")]
        [JsonPropertyName("eventEnd")]
        public DateTime EventEnd { get; set; }

        public string PosterImage { get; set; }

        [JsonPropertyName("categories")]
        public string Categories { get; set; }//json

        [JsonPropertyName("ageLimit")]
        public int AgeLimit { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Количество билетов")]
        [JsonPropertyName("ticketLimit")]
        public int TicketLimit { get; set; }

        public Location Location { get; set; }

        [Display(Name = "Место проведения")]
        public string LocationId { get; set; }
    }
}
