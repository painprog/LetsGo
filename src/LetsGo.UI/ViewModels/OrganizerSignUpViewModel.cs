using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LetsGo.UI.ViewModels
{
    public class OrganizerSignUpViewModel
    {
        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [Remote(action: "CheckUserName", controller: "Account", ErrorMessage = "Это имя пользователя уже занято")]
        [RegularExpression(@"[A-Za-z0-9_.]+$",
            ErrorMessage = "Можно использовать только заглавные, прописные латинские буквы, цифры _ и .")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [Display(Name = "Почта")]
        [EmailAddress(ErrorMessage = "Некорректная почта")]
        [Remote(action: "CheckEmail", controller: "Account", ErrorMessage = "Эта почта уже занята")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [Phone(ErrorMessage = "Неверный формат")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [DataType(DataType.MultilineText)]
        public string SelfInfo { get; set; }

        public IFormFile Avatar { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Повторите пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
