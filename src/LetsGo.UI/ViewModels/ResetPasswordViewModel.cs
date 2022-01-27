using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LetsGo.UI.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [EmailAddress(ErrorMessage = "Некорректная почта")]
        [Remote("EmailChek", "Account", ErrorMessage = "Неправильный email")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Повторите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
