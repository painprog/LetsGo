using LetsGo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.ViewModels
{
    public class AddEventViewModel
    {

        [Required(ErrorMessage = "Поле названия обязательна для заполнения")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле описания обязательна для заполнения")]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Локация")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Время начала")]
        public DateTime EventStart { get; set; }

        [Required(ErrorMessage = "Поле конца времени обязательна для заполнения")]
        [Display(Name = "Время конца")]
        public DateTime EventEnd { get; set; }

        public IList<SelectListItem> Categories { get; set; }

        [Display(Name = "Возрастное ограничение")]
        [Range(0, 18, ErrorMessage = ("Ошибка значения возроста. от 0 до 18"))]
        public string AgeLimit { get; set; }

        public int TicketLimit { get; set; }

        [Display(Name = "Место проведения")]
        [FromForm(Name = "locationId")]
        public string LocationId { get; set; }

        public string OrganizerId { get; set; }

        public string Tickets { get; set; }
        //public List<EventTicketType> Tickets { get; set; }

        //[Required(ErrorMessage = "Название типа билета обязательна")]
        //public string TicketTypeName { get; set; }

        //[Required(ErrorMessage = "Количество типа билета обязательна")]
        //[Range(1, 1000000, ErrorMessage = "Недопустимое значение количества билетов")]
        //public int TicketTypeCount { get; set; }

        //[Required(ErrorMessage = "Цена типа билета обязательна")]
        //[Range(0, 100000000, ErrorMessage = "Недопустимое значение цены билета")]
        //public int TicketTypePrice { get; set; }


        public IFormFile File { get; set; }
    }
}
