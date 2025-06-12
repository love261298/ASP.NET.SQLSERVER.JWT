using ASP.NET.SQLSERVER.JWT.Data;
using ASP.NET.SQLSERVER.JWT.Data.Entity;
using ASP.NET.SQLSERVER.JWT.Data.Modules;
using ASP.NET.SQLSERVER.JWT.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET.SQLSERVER.JWT.Controllers
{
    [ApiController]
    [Route("")]
    public class AuthController(AppDbContext context, TokenService tokenService) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private readonly TokenService _tokenService = tokenService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            {
                return BadRequest("Username already exists.");
            }
            if (dto.ConfirmPassword != dto.Password)
            {
                return BadRequest("Passwords do not match.");
            }
            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

<<<<<<< HEAD
            return Ok(new { message = "User registered." });
=======
            return Ok("User registered.");
>>>>>>> 29b717529d48c47e43a0d479a6466c2d2b48c4df
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid username or password.");

            // Tạo token JWT
            var token = _tokenService.CreateToken(user);

            return Ok(new
            {
                message = "Login successful",
                token,
<<<<<<< HEAD
                role = user.Role,
                userId = user.Id
=======
                role = user.Role
>>>>>>> 29b717529d48c47e43a0d479a6466c2d2b48c4df
            });
        }
    }
}
