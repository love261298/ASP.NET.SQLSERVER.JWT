using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET.SQLSERVER.JWT.Data.Entity
{
    public class Blog
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
<<<<<<< HEAD
        public ICollection<Message>? Messages { get; set; } = new List<Message>();
        public string? ImageUrl { get; set; }
        [ForeignKey("ImageUrl")]
        public Image? Image { get; set; }
=======
        public List<Message>? Messages { get; set; } = new List<Message>();
        public string? CoverImage { get; set; }
>>>>>>> 29b717529d48c47e43a0d479a6466c2d2b48c4df
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    }
}
