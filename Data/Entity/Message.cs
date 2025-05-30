using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET.SQLSERVER.JWT.Data.Entity
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        public Guid? BlogId { get; set; }
        [ForeignKey("BlogId")]
        public Blog? Blog { get; set; }
        [Required]
        public string? Description { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    }
}
