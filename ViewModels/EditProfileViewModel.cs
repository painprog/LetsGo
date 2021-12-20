using System.ComponentModel.DataAnnotations;
using LetsGo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LetsGo.ViewModels
{
    public class EditProfileViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Почта")]
        [Remote(action: "CheckEmail", controller: "Account", ErrorMessage = "Эта почта уже занята")]
        public string Email { get; set; }

    
        [Display(Name = "Имя пользователя")]
        [Remote(action: "CheckUserName", controller: "Account", ErrorMessage = "Это имя пользователя уже занято")]
        public string UserName { get; set; }

      
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        public IFormFile Avatar { get; set; }
    }
}