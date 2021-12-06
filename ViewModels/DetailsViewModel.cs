using LetsGo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.ViewModels
{
    public class DetailsViewModel
    {
        public Event Event { get; set; }
        public List<EventTicketType> EventTickets { get; set; }
        public List<LocationCategory> LocationCategories { get; set; }
        public List<EventCategory> EventCategories { get; set; }
        [Required(ErrorMessage = "Имя пользователя обязательно для заполнения")]
        [Display(Name = "Имя пользоователя")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Номер телефона обязательно для заполнения")]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Почта обязательно для заполнения")]
        [Display(Name = "Почта")]
        [EmailAddress(ErrorMessage = "Некорректно введенная почта")]
        public string Email { get; set; }

        public string EventId { get; set; }
    }
}
