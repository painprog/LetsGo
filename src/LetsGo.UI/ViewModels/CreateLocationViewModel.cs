using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LetsGo.UI.ViewModels
{
    public class CreateLocationViewModel
    {
        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        public string Phones { get; set; }

        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [RegularExpression("[0-9]+[,.]+[0-9]{1,10}", ErrorMessage = "Некорректный значение")]
        public string X { get; set; }
        
        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        [RegularExpression("[0-9]+[,.]+[0-9]{1,10}", ErrorMessage = "Некорректный значение")]
        public string Y { get; set; }


        public IList<SelectListItem> LocationCategories { get; set; }
        public string PhoneNums { get; set; }
    }
}
