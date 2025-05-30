using System.ComponentModel.DataAnnotations;

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

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
    }
}
