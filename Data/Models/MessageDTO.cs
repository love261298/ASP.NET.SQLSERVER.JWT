namespace ASP.NET.SQLSERVER.JWT.Data.Models
{
    public class MessageDTO
    {
        public Guid? BlogId { get; set; }

        public Guid? ConversationId { get; set; }
        public string? Description { get; set; }
    }
}
