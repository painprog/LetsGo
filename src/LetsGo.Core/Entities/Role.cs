using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace LetsGo.Core.Entities
{
    public class Role : IdentityRole<int>
    {
        public ICollection<UserToRole> UserRoles { get; private set; }

        protected Role()
        {
            UserRoles = new Collection<UserToRole>();
        }
    }
}