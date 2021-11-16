using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LetsGo.ViewModels
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


        public IList<SelectListItem> LocationCategories { get; set; }
        public string PhoneNums { get; set; }
    }
}
