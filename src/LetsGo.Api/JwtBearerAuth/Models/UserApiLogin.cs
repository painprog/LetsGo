using System.ComponentModel.DataAnnotations;

namespace LetsGo.Api.JwtBearerAuth.Models
{
    public class UserApiLogin
    {
        [Required] 
        public string UserEmail { get; set; }

        [Required] 
        public string Password { get; set; }
    }
}