using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prueba.Models;
using System.Security.Claims;
using HomeBanking.DTOS;
using HomeBanking.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
                var key = "9Ks3bnBGx8fGJdN7VFnTY8jCDYmq/fR/4V5yVWGzs7Y=";

                if (user.Email.IsNullOrEmpty() || user.Password.IsNullOrEmpty()) { 
                    return StatusCode(403, "Null or empty fields");
                }

                Client client = _clientService.FindByEmail(user.Email);
                if (client == null)
                    return StatusCode(401, "We couldn't validate your email or password");

                string passwordHash = _clientService.PasswordHash(user.Password);

                //comparo los hash de las contraseñas
                if(!string.Equals(passwordHash,client.Password))
                    return StatusCode(401, "We couldn't validate your email or password");

                var claims = new List<Claim>();

                if (user.Email.ToLower().Equals("facudelga3@gmail.com"))
                    claims.Add(new Claim("Admin", client.Email));

                claims.Add(new Claim("Client", client.Email));

                //var claimsIdentity = new ClaimsIdentity(
                //    claims,
                //    CookieAuthenticationDefaults.AuthenticationScheme
                //    );

                var tokenHandler = new JwtSecurityTokenHandler();
                var byteKey = Encoding.UTF8.GetBytes(key);

                var claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

                var tokenDescriptor = new SecurityTokenDescriptor() {
                    Subject = claimsIdentity,
                    Expires = DateTime.UtcNow.AddMinutes(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(byteKey),SecurityAlgorithms.HmacSha256Signature),
                    Issuer = "HB-MH",
                    Audience = "Localhost"
                };

                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(tokenConfig);
                
                //await HttpContext.SignInAsync(
                //    CookieAuthenticationDefaults.AuthenticationScheme,
                //    new ClaimsPrincipal(claimsIdentity));

                return Ok(token);

            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout() {
            try {
                //await HttpContext.SignOutAsync(
                //CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok();
            }
            catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
