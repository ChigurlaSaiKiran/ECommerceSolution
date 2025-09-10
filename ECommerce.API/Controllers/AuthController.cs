using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using ECommerce.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // -------------------- CUSTOMER REGISTER --------------------
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _context.Customers.AnyAsync(c => c.Email == dto.Email))
                return BadRequest("Email already exists");

            var customer = new Customer
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "Customer"
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Registration successful" });
        }

        // -------------------- LOGIN (Customer or Admin) --------------------
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // 1. Try Customer
            var customer = _context.Customers.SingleOrDefault(x => x.Email == request.Email);
            if (customer != null && BCrypt.Net.BCrypt.Verify(request.Password, customer.PasswordHash))
            {
                var token = _jwtService.GenerateToken(customer);
                return Ok(new
                {
                    token,
                    customer.Email,
                    customer.FullName,
                    customer.Role
                });
            }

            // 2. Try Admin User
            var user = _context.Users.SingleOrDefault(x => x.Email == request.Email);
            if (user != null && BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                var token = _jwtService.GenerateToken(user);
                return Ok(new
                {
                    token,
                    user.Email,
                    user.FullName,
                    user.Role
                });
            }

            return Unauthorized("Invalid credentials");
        }
    }
}

//using ECommerce.Application.Services;
//using ECommerce.Domain.Entities;
//using ECommerce.Infrastructure.Data;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;


//namespace ECommerce.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]

//    public class AuthController : ControllerBase
//    {
//        private readonly AppDbContext _context;
//        private readonly JwtService _jwtService;

//        public AuthController(AppDbContext context, JwtService jwtService)
//        {
//            _context = context;
//            _jwtService = jwtService;
//        }

//        [HttpPost("login")]
//        public IActionResult Login([FromBody] LoginRequest request)
//        {
//            var user = _context.Users.SingleOrDefault(x => x.Email == request.Email);
//            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
//            {
//                return Unauthorized("Invalid credentials");
//            }

//            var token = _jwtService.GenerateToken(user);
//            return Ok(new { token, user.Email, user.FullName, user.Role });
//        }

//    }
//}

























//using ECommerce.Application.Services;
//using ECommerce.Domain.Entities;
//using ECommerce.Infrastructure.Data;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//[ApiController]
//[Route("api/[controller]")]
//public class AuthController : ControllerBase
//{
//    private readonly AppDbContext _context;
//    private readonly IConfiguration _config;
//    private readonly JwtService _jwtService;


//    public AuthController(AppDbContext context, IConfiguration config, JwtService jwtService)
//    {
//        _context = context;
//        _config = config;
//        _jwtService = jwtService;
//    }

//    //[HttpPost("login")]
//    //public async Task<IActionResult> Login([FromBody] LoginRequest request)
//    //{
//    //    var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
//    //    if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
//    //    {
//    //        return Unauthorized("Invalid email or password.");
//    //    }

//    //    var token = GenerateJwtToken(user);

//    //    return Ok(new AuthResponse
//    //    {
//    //        Token = token,
//    //        FullName = user.FullName,
//    //        Email = user.Email,
//    //        Role = user.Role
//    //    });
//    //}

//    [HttpPost("login")]
//    public IActionResult Login([FromBody] LoginRequest dto)
//    {
//        var user = _context.Users.SingleOrDefault(x => x.Email == dto.Email);
//        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
//        {
//            return Unauthorized("Invalid credentials");
//        }

//        var token = _jwtService.GenerateToken(user);
//        return Ok(new { token, user.Email, user.FullName, user.Role });
//    }

//    private string GenerateJwtToken(User user)
//    {
//        var jwtSettings = _config.GetSection("Jwt");
//        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

//        var claims = new[]
//        {
//            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
//            new Claim(ClaimTypes.Name, user.FullName),
//            new Claim(ClaimTypes.Email, user.Email),
//            new Claim(ClaimTypes.Role, user.Role)
//        };

//        var token = new JwtSecurityToken(
//            issuer: jwtSettings["Issuer"],
//            audience: jwtSettings["Audience"],
//            claims: claims,
//            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["DurationInMinutes"])),
//            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
//        );

//        return new JwtSecurityTokenHandler().WriteToken(token);
//    }
//}
