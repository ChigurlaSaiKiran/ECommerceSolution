using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerce.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Application.Services
{
    public class JwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        // ✅ Generate Token for both Customer and User
        public string GenerateToken(object entity)
        {
            string id = "";
            string email = "";
            string fullName = "";
            string role = "";

            if (entity is Customer customer)
            {
                id = customer.Id.ToString();
                email = customer.Email;
                fullName = customer.FullName;
                role = customer.Role;
            }
            else if (entity is User user)
            {
                id = user.UserId.ToString();
                email = user.Email;
                fullName = user.FullName;
                role = user.Role;
            }
            else
            {
                throw new ArgumentException("Unsupported entity type for JWT generation");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, id),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim("fullName", fullName),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

//using ECommerce.Domain.Entities;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;

//namespace ECommerce.Application.Services
//{
//    public class JwtService
//    {
//        private readonly IConfiguration _config;

//        public JwtService(IConfiguration config)
//        {
//            _config = config;
//        }

//        public string GenerateToken(User user)
//        {
//            var claims = new[]
//            {
//                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
//                new Claim(ClaimTypes.Name, user.Email),
//                new Claim(ClaimTypes.Role, user.Role)
//            };

//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//            var token = new JwtSecurityToken(
//                  //issuer: _config["JwtSettings:Issuer"],
//                  //audience: _config["JwtSettings:Audience"],
//                  //claims: claims,
//                  //expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["JwtSettings:ExpiresInMinutes"])),
//                  //signingCredentials: creds
//                  issuer: _config["Jwt:Issuer"],
//            audience: _config["Jwt:Audience"],
//            claims: claims,
//            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:DurationInMinutes"])),
//            signingCredentials: creds
//            );

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//    }
//}
