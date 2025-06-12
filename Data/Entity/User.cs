using System.ComponentModel.DataAnnotations;
<<<<<<< HEAD
using System.ComponentModel.DataAnnotations.Schema;
=======
>>>>>>> 29b717529d48c47e43a0d479a6466c2d2b48c4df

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

<<<<<<< HEAD
        public string? ImageUrl { get; set; } = null;
        [ForeignKey("ImageUrl")]
        public Image? Image { get; set; } = null;
        public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
        public ICollection<UserConversation> UserConversations { get; set; } = new List<UserConversation>();

=======
>>>>>>> 29b717529d48c47e43a0d479a6466c2d2b48c4df
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
    }
}
