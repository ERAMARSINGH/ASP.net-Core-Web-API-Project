using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace TestDemo.WebAPI.Controllers
{
    public class TokenController : Controller
    {
        private const string SECRET_KEY = "abcfdyuiotpoiuytre";

        public static readonly SymmetricSecurityKey Signin_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenController.SECRET_KEY));

        [HttpGet]
        [Route("api/token/{UserName}/{Password}")]
        public IActionResult Index(string UserName, string Password)
        {
            if (UserName == Password)
                return new ObjectResult(GenerateToken(UserName));
            else
                return BadRequest();
        }

        private string GenerateToken(string userName)
        {
            var token = new JwtSecurityToken(
                claims: new Claim[]
                {
                    new Claim(ClaimTypes.Name,userName)
                },
                notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: new DateTimeOffset(DateTime.Now.AddMinutes(60)).DateTime,
                signingCredentials: new SigningCredentials(Signin_key, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}