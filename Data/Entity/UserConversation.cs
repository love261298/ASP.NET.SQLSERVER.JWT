using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET.SQLSERVER.JWT.Data.Entity
{
    public class UserConversation
    {
        public Guid? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        public Guid? ConversationId { get; set; }
        [ForeignKey("ConversationId")]
        public Conversation? Conversation { get; set; }
    }
}
