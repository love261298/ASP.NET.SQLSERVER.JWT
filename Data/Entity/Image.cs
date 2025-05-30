using System.ComponentModel.DataAnnotations;

namespace ASP.NET.SQLSERVER.JWT.Data.Entity
{
    public class Image
    {
        [Key]
        [Required]
        public required string ImageUrl { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    }
}
