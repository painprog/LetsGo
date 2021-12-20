using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [EmailAddress]
        [Remote("EmailChek", "Account", ErrorMessage = "Неправильный email")]
        public string Email { get; set; }
    }
}
