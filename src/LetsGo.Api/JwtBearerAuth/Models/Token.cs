using System;

namespace LetsGo.Api.JwtBearerAuth.Models
{
    public class Token
    {
        public string Value { get; set; }

        public string UserName { get; set; }

        public TimeSpan Validaty { get; set; }

        public string RefreshToken { get; set; }

        public int UserId { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public Guid GuidId { get; set; }

        public DateTime ExpiredTime { get; set; }
    }
}