using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JwtAuthApiDemo.Controllers
{
    public class AuthorzeController : Controller
    {
        private readonly JwtSettings _jwtSettings;
        public AuthorzeController(IOptions<JwtSettings> options)
        {
            this._jwtSettings = options.Value;
        }

        [HttpPost]
        public IActionResult GetToken([FromBody]ApiLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (model.Name != "djlnet" || model.Password != "123456")
            {
                return BadRequest();
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"djlnet"),
                new Claim(ClaimTypes.Role,"admin"),
                new Claim("SuperAdmin","true")
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.IssuerSigningKey));

            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
              _jwtSettings.ValidIssuer,
              _jwtSettings.ValidAudience,
              claims,
              DateTime.Now,
              DateTime.Now.AddSeconds(60),
              credentials);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}
