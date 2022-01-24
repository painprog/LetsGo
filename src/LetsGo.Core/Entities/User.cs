using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace LetsGo.Core.Entities
{
    public class User : IdentityUser<int>
    {
        public string AvatarLink { get; set; }
        public string SelfInfo { get; set; }
        public bool Approved { get; set; }

        public ICollection<UserToRole> UserRoles { get; private set; }

        public User()
        {
            UserRoles = new Collection<UserToRole>();
        }
    }
}
