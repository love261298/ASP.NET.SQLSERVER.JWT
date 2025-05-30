using ASP.NET.SQLSERVER.JWT.Data;
using ASP.NET.SQLSERVER.JWT.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET.SQLSERVER.JWT.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConversationsController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpPost]
        public async Task<IActionResult> CreateConversation([FromBody] List<Guid> userIds)
        {
            if (userIds == null || userIds.Count < 2)
                return BadRequest("Phải có ít nhất 2 người dùng.");

            var users = await _context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            if (users.Count != userIds.Count)
                return BadRequest("Một số người dùng không tồn tại.");
            var existingConversation = await _context.Conversations
                .Where(c => c.UserConversations.Count == userIds.Count &&
                            c.UserConversations.All(uc => userIds.Contains(uc.UserId!.Value)) &&
                            userIds.All(id => c.UserConversations.Any(uc => uc.UserId.HasValue && uc.UserId.Value == id)))
                .FirstOrDefaultAsync();

            if (existingConversation != null)
            {
                return Ok(new
                {
                    Message = "Đoạn hội thoại đã tồn tại.",
                    Conversation = existingConversation
                });
            }
            var conversation = new Conversation();
            _context.Conversations.Add(conversation);

            foreach (var userId in userIds)
            {
                _context.UserConversations.Add(new UserConversation
                {
                    UserId = userId,
                    Conversation = conversation
                });
            }
            await _context.SaveChangesAsync();
            return Ok(new
            {
                conversation.Id
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetConversations()
        {
            var conversations = await _context.Conversations.ToListAsync();
            return Ok(conversations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetConversation(Guid id)
        {
            var conversation = await _context.Conversations
                .Include(c => c.UserConversations)
                .ThenInclude(uc => uc.User)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (conversation == null)
                return Ok(new { message = "Chưa có cuộc trò chuyện nào!", conversation });
            return Ok(conversation);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetConversationsByUserId(Guid userId)
        {
            var conversations = await _context.UserConversations
                .Where(uc => uc.UserId == userId)
                .Select(uc => uc.Conversation)
                .ToListAsync();
            if (conversations == null)
                return Ok(new { message = "Chưa có cuộc trò chuyện nào!", conversations });
            return Ok(conversations);
        }
    }
}
