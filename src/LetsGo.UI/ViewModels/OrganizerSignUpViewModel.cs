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

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [Remote(action: "CheckEmail", controller: "Account", ErrorMessage = "Эта почта уже занята")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        public string PhoneNumber { get; set; }

        public IFormFile Avatar { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Повторите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
