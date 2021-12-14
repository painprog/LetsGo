using Microsoft.AspNetCore.Identity;

namespace LetsGo.Core.Entities
{
    public class UserToRole : IdentityUserRole<int>
    {
        public User User { get; private set; }
        public Role Role { get; private set; }
    }
}