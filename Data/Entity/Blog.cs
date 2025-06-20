﻿using System.ComponentModel.DataAnnotations;
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
        public ICollection<Message>? Messages { get; set; } = new List<Message>();
        public string? ImageUrl { get; set; }
        [ForeignKey("ImageUrl")]
        public Image? Image { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    }
}
