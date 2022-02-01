using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LetsGo.ViewModels
{
    public class EditProfileViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле почты обязательна для заполнения")]
        [EmailAddress(ErrorMessage = "Некорректная почта")]
        [Display(Name = "Почта")]
        [Remote(action: "CheckEmailEdit", controller: "Account", ErrorMessage = "Эта почта уже занята")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Поле никнейм обязательна для заполнения")]
        [RegularExpression(@"[A-Za-z0-9_.]+$", 
            ErrorMessage = "Можно использовать только заглавные, прописные латинские буквы, цифры _ и .")]
        [Display(Name = "Никнейм")]
        [Remote(action: "CheckUserNameEdit", controller: "Account", ErrorMessage = "Это имя пользователя уже занято")]
        public string UserName { get; set; }

        [Display(Name = "Номер телефона")]
        [Phone(ErrorMessage = "Неверный формат")]
        public string PhoneNumber { get; set; }

        [Display(Name = "О себе")]
        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        public string SelfInfo { get; set; }

        public IFormFile Avatar { get; set; }
    }
}