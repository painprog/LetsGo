using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace LetsGo.Core.Entities
{
    public class User : IdentityUser<int>
    {
        public string AvatarLink { get; set; }

        public ICollection<UserToRole> UserRoles { get; private set; }

        public User()
        {
            UserRoles = new Collection<UserToRole>();
        }
    }

    public class UserToRole : IdentityUserRole<int>
    {
        public User User { get; private set; }
        public Role Role { get; private set; }
    }
}
