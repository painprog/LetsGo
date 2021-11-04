using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Login или почта")]
        public string LoginOrEmail { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }


        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }
    }
}
