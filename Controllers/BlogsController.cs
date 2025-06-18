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
    public class BlogsController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        [HttpGet]
        public async Task<IActionResult> GetBlogs()
        {
            var blogs = await _context.Blogs
                .Include(b => b.Image)
                .ToListAsync();

            return Ok(blogs);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlog(Guid id)
        {
            var blog = await _context.Blogs
                .Include(b => b.Image)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (blog == null)
                return NotFound();

            return Ok(blog);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBlog([FromBody] BlogDTO blogDTO)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (blogDTO == null || string.IsNullOrEmpty(userIdString))
                return BadRequest();
            if (!Guid.TryParse(userIdString, out Guid userId))
                return BadRequest("Invalid user ID.");
            var blog = new Blog
            {
                UserId = userId,
                Title = blogDTO.Title,
                ImageUrl = blogDTO.ImageUrl,
                Description = blogDTO.Description,
            };
            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();
            return Ok(blog);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlog(Guid id, [FromBody] BlogDTO updatedBlog)
        {
            var blog = await _context.Blogs.FindAsync(id);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid userIdGuid))
                return BadRequest("Invalid user ID.");
            if (blog == null || updatedBlog == null)
                return BadRequest();

            if (userIdGuid != blog.UserId)
                return BadRequest("You don't have permission to perform this action.");
            blog.Title = updatedBlog.Title ?? blog.Title;
            blog.Description = updatedBlog.Description ?? blog.Description;
            blog.ImageUrl = updatedBlog.ImageUrl ?? blog.ImageUrl;
            blog.ModifiedAt = DateTime.UtcNow;
            _context.Entry(blog).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(Guid id)
        {
            var blog = await _context.Blogs
                .Include(b => b.Messages)
                .FirstOrDefaultAsync(b => b.Id == id);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid userIdGuid))
                return BadRequest("Invalid user ID.");
            if (blog == null)
                return NotFound(); ;
            if (blog == null)
                return BadRequest();

            if (userIdGuid != blog.UserId)
                return BadRequest("You don't have permission to perform this action.");
            _context.Messages.RemoveRange(blog.Messages!);
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
