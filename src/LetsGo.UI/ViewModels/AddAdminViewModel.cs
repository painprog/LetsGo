using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LetsGo.UI.ViewModels
{
    public class AddAdminViewModel
    {
        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [Display(Name = "Почта")]
        [EmailAddress(ErrorMessage = "Некорректная почта")]
        [Remote(action: "CheckEmail", controller: "Account", ErrorMessage = "Эта почта уже занята")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [Display(Name = "Имя пользователя")]
        [Remote(action: "CheckUserName", controller: "Account", ErrorMessage = "Это имя пользователя уже занято")]
        [RegularExpression(@"[A-Za-z0-9_.]+$",
            ErrorMessage = "Можно использовать только заглавные, прописные латинские буквы, цифры _ и .")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [Phone(ErrorMessage = "Неверный формат")]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        public IFormFile Avatar { get; set; }

    }
}
