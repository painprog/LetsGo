using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGo.Models
{
    public class Location
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        public string Phones { get; set; }//json

        [Required(ErrorMessage = "Данное поле обязательно для заполнения")]
        public string Description { get; set; }

        public string Categories { get; set; }//json
    }
}
