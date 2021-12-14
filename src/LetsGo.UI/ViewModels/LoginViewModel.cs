using System.ComponentModel.DataAnnotations;

namespace LetsGo.UI.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [Display(Name = "Имя пользователя или почта")]
        public string LoginOrEmail { get; set; }


        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }


        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }
}
