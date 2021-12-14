using System.Security.Claims;
using LetsGo.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace LetsGo.Extensions
{
    public static class UserManagerExtensions
    {
        public static int GetUserIdAsInt(this UserManager<User> userManager, ClaimsPrincipal principal)
        {
            return int.Parse(userManager.GetUserId(principal));
        }
    }
}