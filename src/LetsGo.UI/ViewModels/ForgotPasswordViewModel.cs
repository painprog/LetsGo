using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LetsGo.UI.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [EmailAddress(ErrorMessage = "Некорректная почта")]
        [Remote("EmailChek", "Account", ErrorMessage = "Пользователя с таким email не существует")]
        public string Email { get; set; }
    }
}
