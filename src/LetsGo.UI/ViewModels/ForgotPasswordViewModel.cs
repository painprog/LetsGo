using System.ComponentModel.DataAnnotations;

namespace LetsGo.UI.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
