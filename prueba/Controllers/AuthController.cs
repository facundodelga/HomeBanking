using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prueba.Models;
using System.Security.Claims;
using HomeBanking.DTOS;
using HomeBanking.Services;
using HomeBanking.Util;

namespace HomeBanking.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private IClientService _clientService;

        public AuthController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO user) {
            
            try {
                Client client = _clientService.FindByEmail(user.Email);
                if (client == null)
                    return StatusCode(401, "User info not found.");

                string passwordHash = _clientService.PasswordHash(user.Password);

                if(string.IsNullOrEmpty(passwordHash) || string.Equals(passwordHash,client.Password))
                    return StatusCode(401, "User info not valid");

                var claims = new List<Claim>();

                if (user.Email.ToLower().Equals("facudelga3@gmail.com"))
                    claims.Add(new Claim("Admin", client.Email));

                claims.Add(new Claim("Client", client.Email));

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                    );

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return Ok();

            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout() {
            try {
                await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok();
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
