using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerApp.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;


namespace CustomerAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        CustomerDBContext db = new CustomerDBContext();
        private IConfiguration _config;
        public LoginController(IConfiguration config)
        {
            _config = config;
        }
        [HttpPost]
        [Route("login-user")]
        public IActionResult Login(TblLogin login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login,false);
            if (user != null)
            {
                var tokenString = GenerateToken(user);
                response = Ok(new { token = tokenString });
            }
            return response;
        }

        private TblLogin AuthenticateUser(TblLogin login, bool IsRegister)
        {
            if (IsRegister)
            {
                db.TblLogins.Add(login);
                return login;
            }
            else
            {
                if (db.TblLogins.Any(x => x.Username == login.Username && x.Password == login.Password))
                {
                    return db.TblLogins.Where(x => x.Username == login.Username && x.Password == login.Password).FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
        }

        private string GenerateToken(TblLogin login)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["jwt:Issuer"],
                _config["jwt:Audience"],
                null,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}