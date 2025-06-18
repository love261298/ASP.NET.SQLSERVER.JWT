using System.Security.Claims;
using ASP.NET.SQLSERVER.JWT.Data;
using ASP.NET.SQLSERVER.JWT.Data.Entity;
using ASP.NET.SQLSERVER.JWT.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET.SQLSERVER.JWT.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users
                .Include(u => u.Image)
                .ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.Image)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            user.ModifiedAt = DateTime.UtcNow;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserUpdateDTO updatedUserDto)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (updatedUserDto == null || string.IsNullOrEmpty(userIdString))
                return BadRequest();

            if (!Guid.TryParse(userIdString, out Guid userId) || id != userId)
                return BadRequest("Invalid user ID.");

            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            if (!string.IsNullOrWhiteSpace(updatedUserDto.ImageUrl))
            {
                var image = await _context.Images.FindAsync(updatedUserDto.ImageUrl);
                if (image != null)
                {
                    user.ImageUrl = image.ImageUrl;
                }
            }
            if (!string.IsNullOrWhiteSpace(updatedUserDto.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updatedUserDto.Password);
            }
            if (!string.IsNullOrWhiteSpace(updatedUserDto.Username))
                user.Username = updatedUserDto.Username;

            if (!string.IsNullOrWhiteSpace(updatedUserDto.Role))
                user.Role = updatedUserDto.Role;
            user.ModifiedAt = DateTime.UtcNow;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { user });
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
