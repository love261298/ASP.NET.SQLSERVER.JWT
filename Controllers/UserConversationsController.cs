using System.Security.Claims;
using ASP.NET.SQLSERVER.JWT.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET.SQLSERVER.JWT.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class UserConversationsController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet]
        public async Task<IActionResult> GetUserConversations()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
                return BadRequest("Invalid user ID.");
            var userConversations = await _context.UserConversations.ToListAsync();
            return Ok(userConversations);
        }

        [HttpGet("conversation/{conversationId}")]
        public async Task<IActionResult> GetUserConversation(Guid conversationId)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
                return BadRequest("Invalid user ID.");
            var userConversation = await _context.UserConversations
                .Where(uc => uc.Conversation!.Id == conversationId)
                .ToListAsync();
            if (userConversation == null)
                return NotFound("User conversation not found.");
            return Ok(userConversation);
        }

        [HttpGet("user/{UserId}")]
        public async Task<IActionResult> GetUserConversationsByUserId(Guid UserId)
        {
            var userConversations = await _context.UserConversations
                .Where(uc => uc.UserId == UserId)
                .ToListAsync();
            return Ok(userConversations);
        }
    }
}
