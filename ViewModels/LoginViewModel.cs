using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [Display(Name = "Имя пользователя или почта")]
        [Remote("LoginChek", "Account", ErrorMessage = "Неправильное имя пользователя или email")]
        public string LoginOrEmail { get; set; }


        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }


        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }
}
