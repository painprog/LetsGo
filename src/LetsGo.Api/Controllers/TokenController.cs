using System;
using System.Linq;
using System.Threading.Tasks;
using LetsGo.Api.JwtBearerAuth;
using LetsGo.Api.JwtBearerAuth.Models;
using LetsGo.Core.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LetsGo.UI.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtSettings _jwtSettings;

        public TokenController(UserManager<User> userManager, SignInManager<User> signInManager, JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings;
        }

        [HttpPost("get")]
        public async Task<IActionResult> GetToken([FromBody] UserApiLogin userApiLogin)
        {
            try
            {
                Token token;
                var user = await _userManager.FindByEmailAsync(userApiLogin.UserEmail);
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, userApiLogin.Password, false);
                if (result.Succeeded)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var permittedRole = userRoles.FirstOrDefault(r =>
                        string.Equals(r, _jwtSettings.PermittedRole, StringComparison.OrdinalIgnoreCase));

                    token = JwtTokenHelper.GenToken(user, permittedRole, _jwtSettings);
                }
                else
                {
                    return BadRequest($"Wrong password for user with email {userApiLogin.UserEmail}");
                }
                return Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    $"Server failed to generate token for user with email {userApiLogin.UserEmail}:{Environment.NewLine}{ex}");
            }
        }

        [HttpGet("test")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "apiclient")]
        public IActionResult TestToken()
        {
            return Ok("Token validation succeeded");
        }
    }
}