using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LetsGo.Api.JwtBearerAuth.Models;
using LetsGo.Core.Entities;
using Microsoft.IdentityModel.Tokens;

namespace LetsGo.Api.JwtBearerAuth
{
    public static class JwtTokenHelper
    {
        public static Token GenToken(User user, string role, JwtSettings jwtSettings)
        {
            var token = new Token();

            byte[] key = Encoding.ASCII.GetBytes(jwtSettings.IssuerSigningKey);
            var expireTime = DateTime.UtcNow.AddDays(1);
            token.Validaty = expireTime.TimeOfDay;
            var guid = Guid.NewGuid();
            var jwtToken = new JwtSecurityToken(jwtSettings.ValidIssuer, jwtSettings.ValidAudience,
                GetClaims(user, guid, role), new DateTimeOffset(DateTime.Now).DateTime,
                new DateTimeOffset(expireTime).DateTime,
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));

            token.Value = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            token.UserName = user.UserName;
            token.Email = user.Email;
            token.Role = role;
            token.UserId = user.Id;
            token.GuidId = guid;
            return token;
        }

        private static IEnumerable<Claim> GetClaims(User user, Guid id, string role)
        {
            IEnumerable<Claim> claims = new[]
            {
                new Claim("Id", id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddDays(1).ToString("MMM ddd dd yyyy HH:mm:ss tt"))
            };
            return claims;
        }
    }
}