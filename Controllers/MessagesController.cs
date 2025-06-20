﻿using System.Security.Claims;
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
    public class MessagesController(AppDbContext appDbContext) : ControllerBase
    {
        private readonly AppDbContext _context = appDbContext;
        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _context.Messages.ToListAsync();
            return Ok(messages);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessage(Guid id)
        {
            var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);

            if (message == null)
                return NotFound();

            return Ok(message);
        }
        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] MessageDTO newMessage)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId) || newMessage == null)
                return BadRequest("Invalid user ID.");
            var blogExists = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == newMessage.BlogId);
            var conversationExists = await _context.Conversations.FirstOrDefaultAsync(c => c.Id == newMessage.ConversationId);

            if (blogExists == null && conversationExists == null)
                return NotFound("Blog or conversation not found.");

            var message = new Message
            {
                UserId = userId,
                ConversationId = newMessage.ConversationId ?? null,
                BlogId = newMessage.BlogId ?? null,
                Description = newMessage.Description,
            };
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message.Id,
                message.Description,
                message.UserId,
                message.BlogId,
                message.ConversationId,
                message.CreatedAt,
                message.ModifiedAt
            });
        }
        [HttpPut("{messageId}")]
        public async Task<IActionResult> UpdateMessage(Guid messageId, [FromBody] String Description)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var message = await _context.Messages.FindAsync(messageId);
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId) || message == null)
                return BadRequest();
            if (userId != message.UserId)
                return BadRequest("Message ID mismatch.");
            message.Description = Description;
            message.ModifiedAt = DateTime.UtcNow;
            _context.Messages.Update(message);
            await _context.SaveChangesAsync();
            return Ok(message);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(Guid id)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
                return BadRequest("Invalid user ID.");
            var message = await _context.Messages.FindAsync(id);
            if (message == null || message.UserId != userId)
                return Unauthorized("You do not have permission to delete this message.");
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("blog/{blogId}")]
        public async Task<IActionResult> GetMessagesByBlog(Guid blogId)
        {
            var messages = await _context.Messages
                .Where(m => m.BlogId == blogId)
                .ToListAsync();
            return Ok(messages);
        }

        [HttpGet("conversation/{conversationId}")]
        public async Task<IActionResult> GetMessagesByConversation(Guid conversationId)
        {
            var messages = await _context.Messages
                .Where(m => m.ConversationId == conversationId)
                .ToListAsync();
            return Ok(messages);
        }
    }
}
