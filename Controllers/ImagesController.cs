using ASP.NET.SQLSERVER.JWT.Data;
using ASP.NET.SQLSERVER.JWT.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET.SQLSERVER.JWT.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImagesController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromBody] string ImageUrl)
        {
            if (string.IsNullOrWhiteSpace(ImageUrl))
                return BadRequest("No file uploaded.");
            var image = new Image
            {
                ImageUrl = ImageUrl
            };

            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return Ok(image);
        }
        [HttpGet("Search")]
        public async Task<IActionResult> GetImage([FromQuery] string ImageUrl)
        {
            var image = await _context.Images.FindAsync(ImageUrl);
            if (image == null)
                return Ok(new { code = 0, message = "Not found", image });

            return Ok(new { code = 1, message = "Ok", image });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await _context.Images.ToListAsync();
            return Ok(images);
        }

        [HttpDelete("{ImageUrl}")]
        public async Task<IActionResult> DeleteImage(string ImageUrl)
        {
            var image = await _context.Images.FindAsync(ImageUrl);
            if (image == null)
                return NotFound();
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}