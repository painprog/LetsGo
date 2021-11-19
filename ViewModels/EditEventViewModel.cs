﻿using LetsGo.Enums;
using LetsGo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.ViewModels
{
    public class EditEventViewModel
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PosterImage { get; set; }
        public virtual int StatusId
        {
            get => (int)this.Status;
            set => Status = (Status)value;
        }
        public Status Status { get; set; }
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

        public string Categories { get; set; }

        [Display(Name = "Возрастное ограничение")]
        [Range(0, 18, ErrorMessage = ("Ошибка значения возроста. от 0 до 18"))]
        public string AgeLimit { get; set; }

        public int TicketLimit { get; set; }

        [Display(Name = "Место проведения")]
        [FromForm(Name = "locationId")]
        public string LocationId { get; set; }

        public string OrganizerId { get; set; }

        public string Tickets { get; set; }

        public IFormFile File { get; set; }
    }
}
