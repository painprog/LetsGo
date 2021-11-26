using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.ViewModels
{
    public class AddAdminViewModel
    {
        [Required]
        [Display(Name = "Почта")]
        [Remote(action: "CheckEmail", controller: "Account", ErrorMessage = "Эта почта уже занята")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Имя пользователя")]
        [Remote(action: "CheckUserName", controller: "Account", ErrorMessage = "Это имя пользователя уже занято")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        public IFormFile Avatar { get; set; }

    }
}
