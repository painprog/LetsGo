using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.ViewModels
{
    public class EventViewModel
    {
        [FromForm(Name = "file")]
        public IFormFile File { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Название")]
        [FromForm(Name = "name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Описание")]
        [FromForm(Name = "description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Время начала")]
        [FromForm(Name = "eventStart")]
        public DateTime EventStart { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Время конца")]
        [FromForm(Name = "eventEnd")]
        public DateTime EventEnd { get; set; }

        [FromForm(Name = "categories")]
        public string Categories { get; set; }

        [FromForm(Name = "ageLimit")]
        public string AgeLimit { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Количество билетов")]
        [FromForm(Name = "ticketLimit")]
        public int TicketLimit { get; set; }

        [Display(Name = "Место проведения")]
        [FromForm(Name = "locationId")]
        public string LocationId { get; set; }
    }
}
