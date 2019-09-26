
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Demo.Models;
using Demo.Models.InputModel;
using Demo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Demo.Controllers
{
    [Route("jwt")]
    [Produces("application/json")]
    public class JwtController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        public JwtController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult CreateToken([FromBody] CreateTokenInput input)
        {
            IActionResult response = Unauthorized();
            var user = _userService.Authen(input.Username,input.Password);
            if (user.Id>0)
            {
                var tokenstring = BuildToken(user);
                response = Ok(new {token = tokenstring});
            }
            return response;
        }


        private string BuildToken(Users user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:JwtKey"]));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["jwt:JwtExpireDays"]));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                 _configuration["jwt:JwtIssuer"],
                 _configuration["jwt:JwtAudience"],
                 claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


       
    }
}