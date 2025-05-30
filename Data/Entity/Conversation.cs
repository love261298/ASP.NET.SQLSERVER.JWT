namespace ASP.NET.SQLSERVER.JWT.Data.Entity
{
    public class Conversation
    {
        public Guid Id { get; set; }
        public ICollection<UserConversation> UserConversations { get; set; } = new List<UserConversation>();
        public ICollection<Message>? Messages { get; set; } = new List<Message>();
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastMessageAt { get; set; }
    }
}
