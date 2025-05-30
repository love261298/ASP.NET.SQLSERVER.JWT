using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET.SQLSERVER.JWT.Data.Entity
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }


        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public required string PasswordHash { get; set; }

        [MaxLength(50)]
        public string Role { get; set; } = "User";

        public string? ImageUrl { get; set; } = null;
        [ForeignKey("ImageUrl")]
        public Image? Image { get; set; } = null;
        public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
        public ICollection<UserConversation> UserConversations { get; set; } = new List<UserConversation>();

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
    }
}
