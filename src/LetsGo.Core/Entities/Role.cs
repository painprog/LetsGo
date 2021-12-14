using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace LetsGo.Core.Entities
{
    public sealed class Role : IdentityRole<int>
    {
        public ICollection<UserToRole> UserRoles { get; private set; }

        private Role()
        {
            UserRoles = new Collection<UserToRole>();
        }

        public Role(string name)
        {
            Name = name;
            NormalizedName = name.ToUpper();
            UserRoles = new Collection<UserToRole>();
        }
    }
}